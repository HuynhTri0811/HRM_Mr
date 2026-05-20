using Microsoft.AspNetCore.Builder;
using Serilog;

namespace ApiGateway.Extensions
{
    public static class LoggingExtensions
    {
        public static void ConfigureSerilog(this WebApplicationBuilder builder)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .Enrich.FromLogContext() // Bắt buộc phải có dòng này để lấy dữ liệu từ LogContext.PushProperty
                .CreateLogger();

            builder.Host.UseSerilog();
        }
    }
}
