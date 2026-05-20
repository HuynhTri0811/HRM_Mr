using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Prometheus;
using Serilog;

namespace ChamCongService.Extensions
{
    public static class ObservabilityExtensions
    {
        public static void ConfigureObservability(this WebApplicationBuilder builder)
        {
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
        }

        public static void UseObservability(this WebApplication app)
        {
            app.UseHttpMetrics();
            app.MapMetrics();
        }

        public static async Task CheckMongoAsync(this WebApplication app, IConfiguration configuration)
        {
            try
            {
                string? mongoUrlStr = null;
                var writeToSection = configuration.GetSection("Serilog:WriteTo");
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
                    mongoUrlStr = configuration["Serilog:WriteTo:1:Args:databaseUrl"];
                }

                if (!string.IsNullOrEmpty(mongoUrlStr))
                {
                    var mongoUrl = new MongoUrl(mongoUrlStr);
                    var settings = MongoClientSettings.FromUrl(mongoUrl);
                    settings.ServerSelectionTimeout = TimeSpan.FromSeconds(3); // Giới hạn thời gian kết nối là 3 giây để tránh làm nghẽn quá trình khởi chạy nếu MongoDB bị offline

                    var client = new MongoClient(settings);
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
        }
    }
}
