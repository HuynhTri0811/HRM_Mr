using MediatR;

namespace ChamCongService.Application.Commands
{
    public record CreateBangChamCongTheoThangCommand(int Thang, int Nam, DateTime TuNgay, DateTime DenNgay) : IRequest<Guid>;
    public record UpdateBangChamCongTheoThangCommand(Guid Id, int Thang, int Nam, DateTime TuNgay, DateTime DenNgay, DateTime UpdatedAt) : IRequest<bool>;
    public record DeleteBangChamCongTheoThangCommand(Guid Id) : IRequest<bool>;
    public record ChotBangChamCongCommand(Guid Id, DateTime UpdatedAt) : IRequest<bool>;
    public record MoChotBangChamCongCommand(Guid Id, DateTime UpdatedAt) : IRequest<bool>;
}
