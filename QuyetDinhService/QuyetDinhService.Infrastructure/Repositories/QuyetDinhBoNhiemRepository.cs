using Microsoft.EntityFrameworkCore;
using QuyetDinhService.Domain.Entities;
using QuyetDinhService.QuyetDinhService.Domain.Repositories;
using QuyetDinhService.QuyetDinhService.Infrastructure.Data;

namespace QuyetDinhService.QuyetDinhService.Infrastructure.Repositories
{
    public class QuyetDinhBoNhiemRepository(QuyetDinhDbContext quyetDinhDbContext) : IQuyetDinhBoNhiemRepository
    {
        public async Task AddAsync(QuyetDinhBoNhiem entity)
        {
            await quyetDinhDbContext.QuyetDinhBoNhiems.AddAsync(entity);
            await quyetDinhDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid Oid)
        {
            var entity = await quyetDinhDbContext.QuyetDinhBoNhiems.FindAsync(Oid);
            if (entity != null)
            {
                quyetDinhDbContext.QuyetDinhBoNhiems.Remove(entity);
                await quyetDinhDbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<QuyetDinhBoNhiem?>> GetAllAsync()
        {
            return await quyetDinhDbContext.QuyetDinhBoNhiems.ToArrayAsync();
        }

        public async Task<QuyetDinhBoNhiem?> GetByIdAsync(Guid Oid)
        {
            return await quyetDinhDbContext.QuyetDinhBoNhiems.FindAsync(Oid);
        }

        public async Task SaveChangesAsync()
        {
            await quyetDinhDbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(QuyetDinhBoNhiem entity, DateTime originalUpdatedAt)
        {
            quyetDinhDbContext.Entry(entity).Property(e => e.UpdatedAt).OriginalValue = originalUpdatedAt;
            quyetDinhDbContext.QuyetDinhBoNhiems.Update(entity);
            await quyetDinhDbContext.SaveChangesAsync();
        }
    }
}
