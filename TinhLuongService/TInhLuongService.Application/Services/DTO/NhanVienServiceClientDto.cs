namespace TinhLuongService.Application.Services.DTO
{
    public record NhanVienServiceClientDto(
        Guid Id,
        string MaNhanVien,
        string TenNhanVien,
        string Email,
        decimal LuongCoBan
    );
}
