using Microsoft.EntityFrameworkCore;
using QuanLyNhanSuMicroservice.Core.Repositories;
using QuanLyNhanSuMicroservice.QuanLyNhanVien.Domain.Entities;
using QuanLyNhanSuMicroservice.QuanLyNhanVien.Domain.Repositories;
using QuanLyNhanSuMicroservice.QuanLyNhanVien.Infrastructure.Persistence.Data;

namespace QuanLyNhanSuMicroservice.QuanLyNhanVien.Infrastructure.Repositories
{
    public class VanBangRepository(NhanSuDbContext context) : IVanBangRepository
    {
        public async Task AddAsync(VanBang entity)
        {
            await context.VanBangs.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid Oid)
        {
            var entity = await context.VanBangs.FindAsync(Oid);
            if (entity != null)
            {
                context.VanBangs.Remove(entity);
                await context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<VanBang?>> GetAllAsync()
        {
            return await context.VanBangs.ToArrayAsync();
        }

        public async Task<VanBang?> GetByIdAsync(Guid Oid)
        {
            return await context.VanBangs.FindAsync(Oid);
        }

        public async Task<IEnumerable<VanBang>> GetByNhanVienIdAsync(Guid nhanVienId)
        {
            return await context.VanBangs
                .Where(x => x.GetIdNhanVien() == nhanVienId)
                .ToListAsync();
        }

        public Task SaveChangesAsync()
        {
            return context.SaveChangesAsync();
        }

        public async Task UpdateAsync(VanBang entity)
        {
            context.VanBangs.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
