using Microsoft.EntityFrameworkCore;
using ChamCongService.ChamCongService.Infrastructure.Data;
using ChamCongService.Domain.Entity;
using ChamCongService.Domain.Repositories;

namespace ChamCongService.Infrastructure.Repositories
{
    public class BangChamCongTheoThangRepository(ChamCongDbContext context) : IBangChamCongTheoThangRepository
    {
        public async Task AddAsync(BangChamCongTheoThang entity)
        {
            await context.BangChamCongTheoThangs.AddAsync(entity);
        }

        public async Task DeleteAsync(Guid Oid)
        {
            var entity = await context.BangChamCongTheoThangs.FindAsync(Oid);
            if (entity != null)
            {
                context.BangChamCongTheoThangs.Remove(entity);
            }
        }

        public async Task<IEnumerable<BangChamCongTheoThang?>> GetAllAsync()
        {
            return await context.BangChamCongTheoThangs.ToListAsync();
        }

        public async Task<BangChamCongTheoThang?> GetByIdAsync(Guid Oid)
        {
            return await context.BangChamCongTheoThangs.FindAsync(Oid);
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(BangChamCongTheoThang entity, DateTime originalUpdatedAt)
        {
            context.Entry(entity).Property(e => e.UpdatedAt).OriginalValue = originalUpdatedAt;
            context.BangChamCongTheoThangs.Update(entity);
            await Task.CompletedTask;
        }
    }
}
