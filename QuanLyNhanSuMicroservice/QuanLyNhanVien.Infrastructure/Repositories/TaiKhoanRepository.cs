using Microsoft.EntityFrameworkCore;
using QuanLyNhanSuMicroservice.Core.Repositories;
using QuanLyNhanSuMicroservice.QuanLyNhanVien.Domain.Entities;
using QuanLyNhanSuMicroservice.QuanLyNhanVien.Domain.Repositories;
using QuanLyNhanSuMicroservice.QuanLyNhanVien.Infrastructure.Persistence.Data;

namespace QuanLyNhanSuMicroservice.QuanLyNhanVien.Infrastructure.Repositories
{
    public class TaiKhoanRepository(NhanSuDbContext context) : ITaiKhoanRepository
    {
        public async Task AddAsync(TaiKhoan entity)
        {
            await context.TaiKhoans.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid Oid)
        {
            var entity = await context.TaiKhoans.FindAsync(Oid);
            if (entity != null)
            {
                context.TaiKhoans.Remove(entity);
                await context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<TaiKhoan?>> GetAllAsync()
        {
            return await context.TaiKhoans.ToArrayAsync();
        }

        public async Task<TaiKhoan?> GetByIdAsync(Guid Oid)
        {
            return await context.TaiKhoans.FindAsync(Oid);
        }

        public async Task<TaiKhoan?> GetByUsernameAsync(string username)
        {
            return await context.TaiKhoans.FirstOrDefaultAsync(x => x.Username == username);
        }

        public Task SaveChangesAsync()
        {
            return context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TaiKhoan entity)
        {
            context.TaiKhoans.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
