using Microsoft.EntityFrameworkCore;
using TinhLuongService.Data;
using TinhLuongService.Domain.Entities;
using TinhLuongService.Domain.Repositories;

namespace TinhLuongService.Infrastructure.Repositories
{
    public class KyTinhLuongRepository(TinhLuongDbContext context) : IKyTinhLuongRepositorie
    {
        public async Task AddAsync(KyTinhLuong entity)
        {
            await context.KyTinhLuongs.AddAsync(entity);
        }

        public async Task DeleteAsync(Guid Oid)
        {
            var entity = await context.KyTinhLuongs.FindAsync(Oid);
            if (entity != null)
            {
                context.KyTinhLuongs.Remove(entity);
            }
        }

        public async Task<IEnumerable<KyTinhLuong?>> GetAllAsync()
        {
            return await context.KyTinhLuongs.Include(x => x.NhanVienTinhLuongs).ToListAsync();
        }

        public async Task<KyTinhLuong?> GetByIdAsync(Guid Oid)
        {
            return await context.KyTinhLuongs.Include(x => x.NhanVienTinhLuongs).FirstOrDefaultAsync(x => x.Id == Oid);
        }

        public async Task<KyTinhLuong?> GetByMaKy(string maKy)
        {
            return await context.KyTinhLuongs.Include(x => x.NhanVienTinhLuongs).FirstOrDefaultAsync(x => x.MaKy == maKy);
        }

        public async Task<bool> KhoaKy(Guid id)
        {
            var entity = await context.KyTinhLuongs.FindAsync(id);
            if (entity == null) return false;
            context.Entry(entity).Property(e => e.UpdatedAt).OriginalValue = Common_Date.AddMilliseconds(DateTime.Now);
            entity.KhoaKy();
            context.KyTinhLuongs.Update(entity);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> MoKy(Guid id)
        {
            var entity = await context.KyTinhLuongs.FindAsync(id);
            if (entity == null) return false;
            context.Entry(entity).Property(e => e.UpdatedAt).OriginalValue = Common_Date.AddMilliseconds(DateTime.Now);
            entity.MoKy();
            context.KyTinhLuongs.Update(entity);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }



        public async Task UpdateAsync(KyTinhLuong entity, DateTime originalUpdatedAt)
        {
            context.Entry(entity).Property(e => e.UpdatedAt).OriginalValue = originalUpdatedAt;
            context.KyTinhLuongs.Update(entity);
            await Task.CompletedTask;
        }
    }
}
