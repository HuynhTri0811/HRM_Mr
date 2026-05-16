using ChamCongService.Application.Services.DTO;

namespace ChamCongService.Application.Services
{
    public interface INhanSuServiceClient
    {
        Task<NhanVienServiceClientDto?> GetNhanVienByIdAsync(Guid nhanVienId, string token);
    }
}
