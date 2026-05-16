using ChamCongService.Domain.Entity.Base;

namespace ChamCongService.Domain.Repositories
{
    public interface IBaseRepository<T> where T : ObjectBase
    {
        Task<IEnumerable<T?>> GetAllAsync();
        Task<T?> GetByIdAsync(Guid Oid);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(Guid Oid);
        Task SaveChangesAsync();
    }
}
