using MediatR;
using QuyetDinhService.QuyetDinhService.Application.DTOs.NangLuong;

namespace QuyetDinhService.QuyetDinhService.Application.Query
{
    public record GetAllQuyetDinhNangLuongQuery() : IRequest<IEnumerable<QuyetDinhNangLuongDto>>;
    public record GetQuyetDinhNangLuongByIdQuery(Guid Id) : IRequest<QuyetDinhNangLuongDto?>;
}
