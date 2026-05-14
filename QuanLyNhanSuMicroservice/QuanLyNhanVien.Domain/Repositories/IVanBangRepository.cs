using QuanLyNhanSuMicroservice.QuanLyNhanVien.Domain.Entities;
using QuanLyNhanSuMicroservice.QuanLyNhanVien.Domain.Repositories;

namespace QuanLyNhanSuMicroservice.Core.Repositories
{
    public interface IVanBangRepository : IBaseRepository<VanBang>
    {
        Task<IEnumerable<VanBang>> GetByNhanVienIdAsync(Guid nhanVienId);
    }
}
