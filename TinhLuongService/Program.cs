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

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
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

var app = builder.Build();

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
app.MapControllers();
app.Run();
