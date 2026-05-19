using Microsoft.EntityFrameworkCore;
using ChamCongService.ChamCongService.Infrastructure.Data;
using ChamCongService.Application.Services;

using ChamCongService.Domain.Repositories;
using ChamCongService.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Serilog;
using MongoDB.Driver;
using MongoDB.Bson;
using ChamCongService.API.Filters;
using ChamCongService.API.Filters;
using MassTransit;
using Prometheus;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ResponseLoggingFilter>();
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "ChamCong API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddDbContext<ChamCongDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"),
        x => x.MigrationsHistoryTable("__ChamCongMigrationsHistory")));

// Register Repositories
builder.Services.AddScoped<ILoaiChamCongRepository, LoaiChamCongRepository>();
builder.Services.AddScoped<IPhieuDangKyNghiRepository, PhieuDangKyNghiRepository>();
builder.Services.AddScoped<IBangChamCongTheoThangRepository, BangChamCongTheoThangRepository>();

// Register MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient<INhanSuServiceClient, NhanSuServiceClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["NhanSuApiUrl"] ?? "http://nhansu-api:8081");
});

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMQ:Host"] ?? "localhost", "/", h =>
        {
            h.Username(builder.Configuration["RabbitMQ:Username"] ?? "guest");
            h.Password(builder.Configuration["RabbitMQ:Password"] ?? "guest");
        });
    });
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? "SuperSecretKeyForJwtAuthenticationWhichIsVeryLong!123"))
    };
});

builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource.AddService("ChamCongService"))
    .WithTracing(tracing =>
    {
        tracing
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddSource("MassTransit")
            .AddOtlpExporter(opt =>
            {
                opt.Endpoint = new Uri(builder.Configuration["Otlp:Endpoint"] ?? "http://localhost:4317");
            });
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ChamCongDbContext>();
    context.Database.Migrate();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpMetrics();
app.MapControllers();
app.MapMetrics();

try
{
    Log.Information("Ứng dụng ChamCongService đang khởi động...");

    // Kiểm tra kết nối MongoDB (sử dụng cấu hình từ Serilog)
    try
    {
        string? mongoUrlStr = null;
        var writeToSection = builder.Configuration.GetSection("Serilog:WriteTo");
        foreach (var child in writeToSection.GetChildren())
        {
            if (child["Name"] == "MongoDB")
            {
                mongoUrlStr = child["Args:databaseUrl"];
                break;
            }
        }
        if (string.IsNullOrEmpty(mongoUrlStr))
        {
            mongoUrlStr = builder.Configuration["Serilog:WriteTo:1:Args:databaseUrl"];
        }

        if (!string.IsNullOrEmpty(mongoUrlStr))
        {
            var mongoUrl = new MongoUrl(mongoUrlStr);
            var settings = MongoClientSettings.FromUrl(mongoUrl);
            settings.ServerSelectionTimeout = TimeSpan.FromSeconds(3); // Giới hạn thời gian kết nối là 3 giây để tránh làm nghẽn quá trình khởi chạy nếu MongoDB bị offline

            var client = new MongoClient(settings);
            // Thực hiện lệnh ping để xác nhận kết nối thực tế tới MongoDB
            await client.GetDatabase(mongoUrl.DatabaseName ?? "HrmLogsDb").RunCommandAsync((Command<BsonDocument>)"{ping:1}");
            Log.Information("Kết nối thành công đến MongoDB ({DatabaseName}). Sẵn sàng ghi nhận traffic log.", mongoUrl.DatabaseName);
        }
        else
        {
            Log.Warning("Không tìm thấy cấu hình connection string của MongoDB trong Serilog.");
        }
    }
    catch (Exception ex)
    {
        Log.Error(ex, "Không thể kết nối đến cơ sở dữ liệu MongoDB. Vui lòng kiểm tra trạng thái Docker Container.");
    }

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Ứng dụng ChamCongService gặp lỗi nghiêm trọng khi khởi động.");
}
finally
{
    Log.CloseAndFlush();
}
