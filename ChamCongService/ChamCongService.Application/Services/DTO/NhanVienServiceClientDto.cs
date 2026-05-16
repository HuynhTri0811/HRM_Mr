namespace ChamCongService.Application.Services.DTO
{
    public record NhanVienServiceClientDto(
        Guid Id,
        string MaNhanVien,
        string HoTen,
        string Email
    );
}
