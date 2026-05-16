using MediatR;
using TinhLuongService.Application.DTOs;

namespace TinhLuongService.Application.Query
{
    public record GetAllKyTinhLuongQuery() : IRequest<IEnumerable<KyTinhLuongDto>>;
    public record GetKyTinhLuongByIdQuery(Guid Id) : IRequest<KyTinhLuongDto?>;
}
