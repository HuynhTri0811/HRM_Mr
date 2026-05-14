using MediatR;

namespace QuyetDinhService.QuyetDinhService.Application.Commands
{
    public record DeleteQuyetDinhNangLuongCommand(Guid Id) : IRequest<bool>;
}
