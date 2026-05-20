using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuyetDinhService.QuyetDinhService.Application.Services;
using QuyetDinhService.QuyetDinhService.Domain.Repositories;
using QuyetDinhService.QuyetDinhService.Infrastructure.Data;
using QuyetDinhService.QuyetDinhService.Infrastructure.Repositories;

namespace QuyetDinhService.Extensions
{
    public static class DatabaseExtensions
    {
        public static void ConfigureDatabase(this WebApplicationBuilder builder)
        {
            // Register DbContext
            builder.Services.AddDbContext<QuyetDinhDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"),
                    x => x.MigrationsHistoryTable("__QuyetDinhMigrationsHistory")));

            // Register Repositories
            builder.Services.AddScoped<IQuyetDinhNangLuongRepositoris, QuyetDinhNangLuongRepository>();
            builder.Services.AddScoped<IQuyetDinhBoNhiemRepository, QuyetDinhBoNhiemRepository>();

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
                var context = services.GetRequiredService<QuyetDinhDbContext>();
                context.Database.Migrate();
            }
        }
    }
}
