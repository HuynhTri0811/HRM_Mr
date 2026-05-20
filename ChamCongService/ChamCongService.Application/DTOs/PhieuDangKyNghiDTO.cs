using ChamCongService.Domain.Entity;

namespace ChamCongService.Application.DTOs
{
    public record PhieuDangKyNghiDto(
        Guid Id,
        DateTime NgayNghi,
        string LyDo,
        Guid MaNhanVien,
        Guid LoaiChamCongId,
        string? LoaiChamCongTen,
        HinhThucNghi HinhThuc,
        DateTime UpdatedAt,
        TimeSpan? TuGio = null,
        TimeSpan? DenGio = null,
        LoaiBuoi? LoaiBuoi = null
    );
}
