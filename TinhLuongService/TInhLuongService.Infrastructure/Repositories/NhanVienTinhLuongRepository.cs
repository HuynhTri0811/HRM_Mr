using Microsoft.EntityFrameworkCore;
using TinhLuongService.Data;
using TinhLuongService.Domain.Entities;
using TinhLuongService.Domain.Repositories;

namespace TinhLuongService.Infrastructure.Repositories
{
    public class NhanVienTinhLuongRepository(TinhLuongDbContext context) : INhanVienTinhLuongRepositorie
    {
        public async Task AddAsync(NhanVienTinhLuong entity)
        {
            await context.NhanVienTinhLuongs.AddAsync(entity);
        }

        public async Task DeleteAsync(Guid Oid)
        {
            var entity = await context.NhanVienTinhLuongs.FindAsync(Oid);
            if (entity != null)
            {
                context.NhanVienTinhLuongs.Remove(entity);
            }
        }

        public async Task<IEnumerable<NhanVienTinhLuong?>> GetAllAsync()
        {
            return await context.NhanVienTinhLuongs.ToListAsync();
        }

        public async Task<NhanVienTinhLuong?> GetByIdAsync(Guid Oid)
        {
            return await context.NhanVienTinhLuongs.FirstOrDefaultAsync(x => x.Id == Oid);
        }

        public async Task<IEnumerable<NhanVienTinhLuong?>> GetByKyTinhLuongId(Guid kyTinhLuongId)
        {
            return await context.NhanVienTinhLuongs.Where(x => x.KyTinhLuongId == kyTinhLuongId).ToListAsync();
        }

        public async Task<NhanVienTinhLuong?> GetByNhanVienIdAndKyTinhLuongId(Guid nhanVienId, Guid kyTinhLuongId)
        {
            return await context.NhanVienTinhLuongs.FirstOrDefaultAsync(x => x.NhanVienId == nhanVienId && x.KyTinhLuongId == kyTinhLuongId);
        }


        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }


        public async Task UpdateAsync(NhanVienTinhLuong entity)
        {
            context.NhanVienTinhLuongs.Update(entity);
            await Task.CompletedTask;
        }

        public async Task UpdateAsync(NhanVienTinhLuong entity, DateTime originalUpdatedAt)
        {
            context.Entry(entity).Property(e => e.UpdatedAt).OriginalValue = originalUpdatedAt;
            context.NhanVienTinhLuongs.Update(entity);
            await Task.CompletedTask;
        }
    }
}
