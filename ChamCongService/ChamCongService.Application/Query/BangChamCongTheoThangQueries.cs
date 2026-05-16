using MediatR;
using ChamCongService.Application.DTOs;

namespace ChamCongService.Application.Query
{
    public record GetAllBangChamCongTheoThangQuery() : IRequest<IEnumerable<BangChamCongTheoThangDto>>;
    public record GetBangChamCongTheoThangByIdQuery(Guid Id) : IRequest<BangChamCongTheoThangDto?>;
}
