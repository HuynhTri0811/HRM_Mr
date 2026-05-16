using QuyetDinhService.QuyetDinhService.Application.Services.DTO;
namespace QuyetDinhService.QuyetDinhService.Application.Services
{

    public interface INhanSuServiceClient
    {
        Task<bool> UpdateLuongAsync(Guid nhanVienId, decimal luongMoi, string token);
        Task<bool> UpdateBoNhiemAsync(Guid nhanVienId, Guid chucVuMoi, decimal phuCapMoi, string token);
        Task<NhanVienServiceClientDto?> GetNhanVienByIdAsync(Guid nhanVienId, string token);
        Task<ChucVuServiceClientDto?> GetChucVuByIdAsync(Guid chucVuId, string token);
    }
}