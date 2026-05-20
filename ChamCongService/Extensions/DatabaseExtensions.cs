using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ChamCongService.Application.Services;
using ChamCongService.ChamCongService.Infrastructure.Data;
using ChamCongService.Domain.Repositories;
using ChamCongService.Infrastructure.Repositories;

namespace ChamCongService.Extensions
{
    public static class DatabaseExtensions
    {
        public static void ConfigureDatabase(this WebApplicationBuilder builder)
        {
            // Register DbContext
            builder.Services.AddDbContext<ChamCongDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"),
                    x => x.MigrationsHistoryTable("__ChamCongMigrationsHistory")));

            // Register Repositories
            builder.Services.AddScoped<ILoaiChamCongRepository, LoaiChamCongRepository>();
            builder.Services.AddScoped<IPhieuDangKyNghiRepository, PhieuDangKyNghiRepository>();
            builder.Services.AddScoped<IBangChamCongTheoThangRepository, BangChamCongTheoThangRepository>();

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
                var context = services.GetRequiredService<ChamCongDbContext>();
                context.Database.Migrate();
            }
        }
    }
}
