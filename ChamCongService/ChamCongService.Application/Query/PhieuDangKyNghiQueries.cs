using MediatR;
using ChamCongService.Application.DTOs;

namespace ChamCongService.Application.Query
{
    public record GetAllPhieuDangKyNghiQuery() : IRequest<IEnumerable<PhieuDangKyNghiDto>>;
    public record GetPhieuDangKyNghiByIdQuery(Guid Id) : IRequest<PhieuDangKyNghiDto?>;
}
