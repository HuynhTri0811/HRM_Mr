using TinhLuongService.Domain.Entities;

namespace TinhLuongService.Domain.Repositories
{
    public interface IKyTinhLuongRepositorie : IBaseRepository<KyTinhLuong>
    {
        Task<KyTinhLuong?> GetByMaKy(string maKy);
        Task<bool> KhoaKy(Guid id);
        Task<bool> MoKy(Guid id);
    }
}