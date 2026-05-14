using Microsoft.EntityFrameworkCore;
using QuanLyNhanSuMicroservice.QuanLyNhanVien.Domain.Entities;
using QuanLyNhanSuMicroservice.QuanLyNhanVien.Domain.Repositories;
using QuanLyNhanSuMicroservice.QuanLyNhanVien.Infrastructure.Persistence.Data;

namespace QuanLyNhanSuMicroservice.QuanLyNhanVien.Infrastructure.Repositories
{
    public class NhanVienRepository(NhanSuDbContext context) : INhanVienRepository
    {
        public async Task AddAsync(NhanVien entity)
        {
            await context.NhanViens.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid Oid)
        {
            var entity = await context.NhanViens.FindAsync(Oid);
            if (entity != null)
            {
                context.NhanViens.Remove(entity);
                await context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<NhanVien?>> GetAllAsync()
        {
            return await context.NhanViens
                .Include(x => x.PhongBan)
                .Include(x => x.ChucVu)
                .Include(x => x.VanBangs)
                .ToArrayAsync();
        }

        public async Task<NhanVien?> GetByIdAsync(Guid Oid)
        {
            return await context.NhanViens
                .Include(x => x.PhongBan)
                .Include(x => x.ChucVu)
                .Include(x => x.VanBangs)
                .FirstOrDefaultAsync(x => x.Id == Oid);
        }

        public Task SaveChangesAsync()
        {
            return context.SaveChangesAsync();
        }

        public async Task UpdateAsync(NhanVien entity)
        {
            context.NhanViens.Update(entity);
            await context.SaveChangesAsync();
        }
        public async Task<bool> IsEmailUniqueAsync(string email)
        {
            return !await context.NhanViens.AnyAsync(x => x.Email == email);
        }
    }
}
