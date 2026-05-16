namespace ChamCongService.Application.DTOs
{
    public record BangChamCongTheoThangDto(
        Guid Id,
        int Thang,
        int Nam,
        DateTime TuNgay,
        DateTime DenNgay,
        bool IsChot
    );
}
