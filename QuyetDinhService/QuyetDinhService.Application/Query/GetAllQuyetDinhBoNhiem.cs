using MediatR;
using QuyetDinhService.QuyetDinhService.Application.DTOs.BoNhiem;

namespace QuyetDinhService.QuyetDinhService.Application.Query
{
    public record GetAllQuyetDinhBoNhiemQuery() : IRequest<IEnumerable<QuyetDinhBoNhiemDto>>;
    public record GetQuyetDinhBoNhiemByIdQuery(Guid Id) : IRequest<QuyetDinhBoNhiemDto?>;
}
