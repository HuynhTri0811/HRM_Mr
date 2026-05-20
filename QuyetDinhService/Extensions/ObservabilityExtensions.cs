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

namespace QuyetDinhService.Extensions
{
    public static class ObservabilityExtensions
    {
        public static void ConfigureObservability(this WebApplicationBuilder builder)
        {
            builder.Services.AddOpenTelemetry()
                .ConfigureResource(resource => resource.AddService("QuyetDinhService"))
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
                var mongoUrl = configuration["Serilog:WriteTo:1:Args:databaseUrl"];
                if (!string.IsNullOrEmpty(mongoUrl))
                {
                    var client = new MongoClient(mongoUrl);
                    var database = client.GetDatabase("HrmLogsDb");
                    using (var cts = new System.Threading.CancellationTokenSource(TimeSpan.FromSeconds(3)))
                    {
                        await database.RunCommandAsync((Command<BsonDocument>)"{ping: 1}", cancellationToken: cts.Token);
                    }
                    Log.Information("QuyetDinhService successfully connected to MongoDB.");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "QuyetDinhService failed to connect to MongoDB at startup.");
            }
        }
    }
}
