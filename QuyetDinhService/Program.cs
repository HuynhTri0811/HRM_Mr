using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using QuyetDinhService.QuyetDinhService.Application.Services;
using QuyetDinhService.QuyetDinhService.Infrastructure.Data;
using QuyetDinhService.QuyetDinhService.Domain.Repositories;
using QuyetDinhService.QuyetDinhService.Infrastructure.Repositories;
using Serilog;
using MongoDB.Driver;
using MongoDB.Bson;
using QuyetDinhService.API.Filters;

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
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "QuyetDinh API", Version = "v1" });
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

builder.Services.AddDbContext<QuyetDinhDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"),
        x => x.MigrationsHistoryTable("__QuyetDinhMigrationsHistory")));

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

builder.Services.AddHttpContextAccessor();

builder.Services.AddHttpClient<INhanSuServiceClient, NhanSuServiceClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["NhanSuApiUrl"] ?? "http://nhansu-api:8081");
});

// Register Repositories
builder.Services.AddScoped<IQuyetDinhNangLuongRepositoris, QuyetDinhNangLuongRepository>();
builder.Services.AddScoped<IQuyetDinhBoNhiemRepository, QuyetDinhBoNhiemRepository>();

// Register MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

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
        Log.Information("QuyetDinhService successfully connected to MongoDB.");
    }
}
catch (Exception ex)
{
    Log.Error(ex, "QuyetDinhService failed to connect to MongoDB at startup.");
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<QuyetDinhDbContext>();
    context.Database.Migrate();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
