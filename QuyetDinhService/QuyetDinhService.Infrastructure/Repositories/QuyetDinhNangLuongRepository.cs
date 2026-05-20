using Microsoft.EntityFrameworkCore;
using QuyetDinhService.Domain.Entities;
using QuyetDinhService.QuyetDinhService.Domain.Repositories;
using QuyetDinhService.QuyetDinhService.Infrastructure.Data;

namespace QuyetDinhService.QuyetDinhService.Infrastructure.Repositories
{
    public class QuyetDinhNangLuongRepository(QuyetDinhDbContext quyetDinhDbContext) : IQuyetDinhNangLuongRepositoris
    {
        public async Task AddAsync(QuyetDinhNangLuong entity)
        {
            await quyetDinhDbContext.QuyetDinhNangLuongs.AddAsync(entity);
            await quyetDinhDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid Oid)
        {
            var entity = await quyetDinhDbContext.QuyetDinhNangLuongs.FindAsync(Oid);
            if (entity != null)
            {
                quyetDinhDbContext.QuyetDinhNangLuongs.Remove(entity);
                await quyetDinhDbContext.SaveChangesAsync();
            }   
        }

        public async Task<IEnumerable<QuyetDinhNangLuong?>> GetAllAsync()
        {
            return await quyetDinhDbContext.QuyetDinhNangLuongs.ToArrayAsync();
        }

        public async Task<QuyetDinhNangLuong?> GetByIdAsync(Guid Oid)
        {
            return await quyetDinhDbContext.QuyetDinhNangLuongs.FindAsync(Oid);
        }   

        public async Task SaveChangesAsync()
        {
            await quyetDinhDbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(QuyetDinhNangLuong entity, DateTime originalUpdatedAt)
        {
            quyetDinhDbContext.Entry(entity).Property(e => e.UpdatedAt).OriginalValue = originalUpdatedAt;
            quyetDinhDbContext.QuyetDinhNangLuongs.Update(entity);
            await quyetDinhDbContext.SaveChangesAsync();    
        }
    }
}
