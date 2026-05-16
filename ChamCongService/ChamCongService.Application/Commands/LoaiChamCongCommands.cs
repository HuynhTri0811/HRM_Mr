using MediatR;
using ChamCongService.Domain.Entity;

namespace ChamCongService.Application.Commands
{
    public record CreateLoaiChamCongCommand(string MaLoai, string TenLoai, double HeSo, HinhThucNghi HinhThuc) : IRequest<Guid>;
    public record UpdateLoaiChamCongCommand(Guid Id, string MaLoai, string TenLoai, double HeSo, HinhThucNghi HinhThuc) : IRequest<bool>;
    public record DeleteLoaiChamCongCommand(Guid Id) : IRequest<bool>;
}
