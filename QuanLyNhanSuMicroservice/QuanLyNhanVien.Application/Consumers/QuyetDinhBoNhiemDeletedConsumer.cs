using MassTransit;
using MediatR;
using QuyetDinhService.QuyetDinhService.Application.Events;
using QuanLyNhanSuMicroservice.QuanLyNhanVien.Application.Command.NhanSu;
using System.Threading.Tasks;

namespace QuanLyNhanSuMicroservice.QuanLyNhanVien.Application.Consumers
{
    public class QuyetDinhBoNhiemDeletedConsumer(IMediator mediator) : IConsumer<QuyetDinhBoNhiemDeletedEvent>
    {
        public async Task Consume(ConsumeContext<QuyetDinhBoNhiemDeletedEvent> context)
        {
            var message = context.Message;

            // Restore the old position when a decision is deleted
            await mediator.Send(new UpdateNhanVienBoNhiemDTO(
                message.MaNhanVien,
                message.ChucVuCu,
                message.PhuCapCu
            ));
        }
    }
}
