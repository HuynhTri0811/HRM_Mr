using TinhLuongService.Domain.Entities;

namespace TinhLuongService.Domain.Repositories
{
    public interface INhanVienTinhLuongRepositorie : IBaseRepository<NhanVienTinhLuong>
    {
        Task<IEnumerable<NhanVienTinhLuong?>> GetByKyTinhLuongId(Guid kyTinhLuongId);
        Task<NhanVienTinhLuong?> GetByNhanVienIdAndKyTinhLuongId(Guid nhanVienId, Guid kyTinhLuongId);
    }
}