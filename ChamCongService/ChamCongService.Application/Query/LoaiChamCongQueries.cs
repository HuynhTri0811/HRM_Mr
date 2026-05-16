using MediatR;
using ChamCongService.Application.DTOs;

namespace ChamCongService.Application.Query
{
    public record GetAllLoaiChamCongQuery() : IRequest<IEnumerable<LoaiChamCongDto>>;
    public record GetLoaiChamCongByIdQuery(Guid Id) : IRequest<LoaiChamCongDto?>;
}
