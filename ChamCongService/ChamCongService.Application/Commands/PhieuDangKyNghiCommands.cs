using MediatR;
using ChamCongService.Domain.Entity;

namespace ChamCongService.Application.Commands
{
    public record CreatePhieuDangKyNghiCommand(
        DateTime NgayNghi,
        string LyDo,
        Guid MaNhanVien,
        Guid LoaiChamCongId,
        TimeSpan? TuGio = null,
        TimeSpan? DenGio = null,
        LoaiBuoi? LoaiBuoi = null
    ) : IRequest<Guid>;

    public record UpdatePhieuDangKyNghiCommand(
        Guid Id,
        DateTime NgayNghi,
        string LyDo,
        Guid MaNhanVien,
        Guid LoaiChamCongId,
        DateTime UpdatedAt,
        TimeSpan? TuGio = null,
        TimeSpan? DenGio = null,
        LoaiBuoi? LoaiBuoi = null
    ) : IRequest<bool>;

    public record DeletePhieuDangKyNghiCommand(Guid Id) : IRequest<bool>;
}
