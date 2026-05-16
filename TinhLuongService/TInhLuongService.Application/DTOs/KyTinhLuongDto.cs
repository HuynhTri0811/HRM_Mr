namespace TinhLuongService.Application.DTOs
{
    public record KyTinhLuongDto(
        Guid Id,
        string MaKy,
        DateTime NgayBatDau,
        DateTime NgayKetThuc,
        bool ChotTinhLuong
    );
}
