using Microsoft.EntityFrameworkCore;
using ChamCongService.ChamCongService.Infrastructure.Data;
using ChamCongService.Domain.Entity;
using ChamCongService.Domain.Repositories;

namespace ChamCongService.Infrastructure.Repositories
{
    public class LoaiChamCongRepository(ChamCongDbContext context) : ILoaiChamCongRepository
    {
        public async Task AddAsync(LoaiChamCong entity)
        {
            await context.LoaiChamCongs.AddAsync(entity);
        }

        public async Task DeleteAsync(Guid Oid)
        {
            var entity = await context.LoaiChamCongs.FindAsync(Oid);
            if (entity != null)
            {
                context.LoaiChamCongs.Remove(entity);
            }
        }

        public async Task<IEnumerable<LoaiChamCong?>> GetAllAsync()
        {
            return await context.LoaiChamCongs.ToListAsync();
        }

        public async Task<LoaiChamCong?> GetByIdAsync(Guid Oid)
        {
            return await context.LoaiChamCongs.FindAsync(Oid);
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(LoaiChamCong entity)
        {
            context.LoaiChamCongs.Update(entity);
            await Task.CompletedTask;
        }
    }
}
