using Microsoft.EntityFrameworkCore;
using TinhLuongService.Data;
using TinhLuongService.Domain.Repositories;
using TinhLuongService.Infrastructure.Repositories;
using TinhLuongService.Domain.Service;
using TinhLuongService.Domain.Service.Interface;
using TinhLuongService.Application.Services.Interface;
using TinhLuongService.Application.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Serilog;
using MongoDB.Driver;
using MongoDB.Bson;
using TinhLuongService.API.Filters;
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
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "TinhLuong API", Version = "v1" });
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
builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<TinhLuongDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"),
        x => x.MigrationsHistoryTable("__TinhLuongMigrationsHistory")));

// Register Repositories
builder.Services.AddScoped<IKyTinhLuongRepositorie, KyTinhLuongRepository>();
builder.Services.AddScoped<INhanVienTinhLuongRepositorie, NhanVienTinhLuongRepository>();

// Register Services
builder.Services.AddScoped<ITinhLuongService, TinhLuongService.Domain.Service.TinhLuongService>();

builder.Services.AddHttpClient<INhanSuServiceClient, NhanSuServiceClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["NhanSuApiUrl"] ?? "http://nhansu-api:8081");
});

// Register MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

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
    .ConfigureResource(resource => resource.AddService("TinhLuongService"))
    .WithTracing(tracing =>
    {
        tracing
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            // .AddSource("MassTransit") // TinhLuong doesn't use MassTransit yet based on its code, but it's safe to add if it ever does. I'll omit it since MassTransit using is not at the top. Wait, let's just add it just in case, it doesn't hurt. No, better to omit if MassTransit is not installed.
            .AddOtlpExporter(opt =>
            {
                opt.Endpoint = new Uri(builder.Configuration["Otlp:Endpoint"] ?? "http://localhost:4317");
            });
    });

var app = builder.Build();

try
{
    var mongoUrl = builder.Configuration["Serilog:WriteTo:1:Args:databaseUrl"];
    if (!string.IsNullOrEmpty(mongoUrl))
    {
        var client = new MongoClient(mongoUrl);
        var database = client.GetDatabase("HrmLogsDb");
        using (var cts = new System.Threading.CancellationTokenSource(TimeSpan.FromSeconds(3)))
        {
            database.RunCommand((Command<BsonDocument>)"{ping: 1}", cancellationToken: cts.Token);
        }
        Log.Information("TinhLuongService successfully connected to MongoDB.");
    }
}
catch (Exception ex)
{
    Log.Error(ex, "TinhLuongService failed to connect to MongoDB at startup.");
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<TinhLuongDbContext>();
    context.Database.Migrate();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpMetrics();
app.MapControllers();
app.MapMetrics();
app.Run();
