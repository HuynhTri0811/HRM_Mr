using QuanLyNhanSuMicroservice.QuanLyNhanVien.Domain.Entities;
using QuanLyNhanSuMicroservice.QuanLyNhanVien.Domain.Repositories;

namespace QuanLyNhanSuMicroservice.Core.Repositories
{
    public interface ITaiKhoanRepository : IBaseRepository<TaiKhoan>
    {
        Task<TaiKhoan?> GetByUsernameAsync(string username);
    }
}
