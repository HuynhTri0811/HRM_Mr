using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuanLyNhanSuMicroservice.QuanLyNhanVien.Application.Consumers;

namespace QuanLyNhanSuMicroservice.Extensions
{
    public static class MessagingExtensions
    {
        public static void ConfigureMessaging(this WebApplicationBuilder builder)
        {
            builder.Services.AddMassTransit(x =>
            {
                x.AddConsumer<QuyetDinhBoNhiemCreatedConsumer>();
                x.AddConsumer<QuyetDinhBoNhiemDeletedConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(builder.Configuration["RabbitMQ:Host"] ?? "localhost", "/", h =>
                    {
                        h.Username(builder.Configuration["RabbitMQ:Username"] ?? "guest");
                        h.Password(builder.Configuration["RabbitMQ:Password"] ?? "guest");
                    });

                    cfg.ReceiveEndpoint("nhansu-quyetdinh-bonhiem-created", e =>
                    {
                        e.ConfigureConsumer<QuyetDinhBoNhiemCreatedConsumer>(context);
                    });

                    cfg.ReceiveEndpoint("nhansu-quyetdinh-bonhiem-deleted", e =>
                    {
                        e.ConfigureConsumer<QuyetDinhBoNhiemDeletedConsumer>(context);
                    });
                });
            });
        }
    }
}
