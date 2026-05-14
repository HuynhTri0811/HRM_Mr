using Microsoft.EntityFrameworkCore;
using QuanLyNhanSuMicroservice.Core.Repositories;
using QuanLyNhanSuMicroservice.QuanLyNhanVien.Domain.Entities;
using QuanLyNhanSuMicroservice.QuanLyNhanVien.Domain.Repositories;
using QuanLyNhanSuMicroservice.QuanLyNhanVien.Infrastructure.Persistence.Data;

namespace QuanLyNhanSuMicroservice.QuanLyNhanVien.Infrastructure.Repositories
{
    public class PhongBanRepository(NhanSuDbContext context) : IPhongBanRepository
    {
        public async Task AddAsync(PhongBan entity)
        {
            await context.PhongBans.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid Oid)
        {
            var entity = await context.PhongBans.FindAsync(Oid);
            if (entity != null)
            {
                context.PhongBans.Remove(entity);
                await context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<PhongBan?>> GetAllAsync()
        {
            return await context.PhongBans.ToArrayAsync();
        }

        public async Task<PhongBan?> GetByIdAsync(Guid Oid)
        {
            return await context.PhongBans.FindAsync(Oid);
        }

        public Task SaveChangesAsync()
        {
            return context.SaveChangesAsync();
        }

        public async Task UpdateAsync(PhongBan entity)
        {
            context.PhongBans.Update(entity);
            await context.SaveChangesAsync();
        }


    }
}
