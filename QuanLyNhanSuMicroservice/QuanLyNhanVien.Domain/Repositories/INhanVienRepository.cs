using QuanLyNhanSuMicroservice.QuanLyNhanVien.Domain.Entities;
using QuanLyNhanSuMicroservice.QuanLyNhanVien.Domain.Repositories;

namespace QuanLyNhanSuMicroservice.QuanLyNhanVien.Domain.Repositories
{
    public interface INhanVienRepository : IBaseRepository<NhanVien>
    {
        Task<bool> IsEmailUniqueAsync(string email);
    }

}
