using MassTransit;
using MediatR;
using QuanLyNhanSuMicroservice.QuyetDinhService.Application.Events;
using QuanLyNhanSuMicroservice.QuanLyNhanVien.Application.Command.NhanSu;
using System.Threading.Tasks;

namespace QuanLyNhanSuMicroservice.QuanLyNhanVien.Application.Consumers
{
    public class QuyetDinhBoNhiemCreatedConsumer(IMediator mediator) : IConsumer<QuyetDinhBoNhiemCreatedEvent>
    {
        public async Task Consume(ConsumeContext<QuyetDinhBoNhiemCreatedEvent> context)
        {
            var message = context.Message;

            // Update employee's position using the existing MediatR handler
            await mediator.Send(new UpdateNhanVienBoNhiemDTO(
                message.MaNhanVien,
                message.ChucVuMoi,
                message.PhuCapMoi
            ));
        }
    }
}
