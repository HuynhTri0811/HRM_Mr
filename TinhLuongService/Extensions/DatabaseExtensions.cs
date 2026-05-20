using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TinhLuongService.Application.Services;
using TinhLuongService.Application.Services.Interface;
using TinhLuongService.Data;
using TinhLuongService.Domain.Repositories;
using TinhLuongService.Domain.Service;
using TinhLuongService.Domain.Service.Interface;
using TinhLuongService.Infrastructure.Repositories;

namespace TinhLuongService.Extensions
{
    public static class DatabaseExtensions
    {
        public static void ConfigureDatabase(this WebApplicationBuilder builder)
        {
            // Register DbContext
            builder.Services.AddDbContext<TinhLuongDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"),
                    x => x.MigrationsHistoryTable("__TinhLuongMigrationsHistory")));

            // Register Repositories
            builder.Services.AddScoped<IKyTinhLuongRepositorie, KyTinhLuongRepository>();
            builder.Services.AddScoped<INhanVienTinhLuongRepositorie, NhanVienTinhLuongRepository>();

            // Register Services
            builder.Services.AddScoped<ITinhLuongService, TinhLuongService.Domain.Service.TinhLuongService>();

            // Register HttpClient Client
            builder.Services.AddHttpClient<INhanSuServiceClient, NhanSuServiceClient>(client =>
            {
                client.BaseAddress = new Uri(builder.Configuration["NhanSuApiUrl"] ?? "http://nhansu-api:8081");
            });

            // Register MediatR
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
        }

        public static void MigrateDatabase(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<TinhLuongDbContext>();
                context.Database.Migrate();
            }
        }
    }
}
