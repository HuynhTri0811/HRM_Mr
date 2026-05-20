using Microsoft.EntityFrameworkCore;
using ChamCongService.ChamCongService.Infrastructure.Data;
using ChamCongService.Domain.Entity;
using ChamCongService.Domain.Repositories;

namespace ChamCongService.Infrastructure.Repositories
{
    public class PhieuDangKyNghiRepository(ChamCongDbContext context) : IPhieuDangKyNghiRepository
    {
        public async Task AddAsync(PhieuDangKyNghi entity)
        {
            await context.PhieuDangKyNghis.AddAsync(entity);
        }

        public async Task DeleteAsync(Guid Oid)
        {
            var entity = await context.PhieuDangKyNghis.FindAsync(Oid);
            if (entity != null)
            {
                context.PhieuDangKyNghis.Remove(entity);
            }
        }

        public async Task<IEnumerable<PhieuDangKyNghi?>> GetAllAsync()
        {
            return await context.PhieuDangKyNghis
                .Include(x => x.LoaiChamCong)
                .ToListAsync();
        }

        public async Task<PhieuDangKyNghi?> GetByIdAsync(Guid Oid)
        {
            return await context.PhieuDangKyNghis
                .Include(x => x.LoaiChamCong)
                .FirstOrDefaultAsync(x => x.Id == Oid);
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(PhieuDangKyNghi entity, DateTime originalUpdatedAt)
        {
            context.Entry(entity).Property(e => e.UpdatedAt).OriginalValue = originalUpdatedAt;
            context.PhieuDangKyNghis.Update(entity);
            await Task.CompletedTask;
        }
    }
}
