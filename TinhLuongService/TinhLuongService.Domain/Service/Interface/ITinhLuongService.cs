using TinhLuongService.Domain.Entities;

namespace TinhLuongService.Domain.Service.Interface
{
    public interface ITinhLuongService
    {
        void TinhLuong(KyTinhLuong kyTinhLuong, IList<NhanVienTinhLuong> nhanVienTinhLuongs);
    }
}