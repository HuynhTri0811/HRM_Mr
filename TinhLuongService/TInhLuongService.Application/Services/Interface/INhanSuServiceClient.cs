using TinhLuongService.Application.Services.DTO;

namespace TinhLuongService.Application.Services.Interface
{
    public interface INhanSuServiceClient
    {
        Task<IEnumerable<NhanVienServiceClientDto>> GetAllNhanVienAsync(string token);
    }
}
