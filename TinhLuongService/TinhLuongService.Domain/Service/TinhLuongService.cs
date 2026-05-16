using TinhLuongService.Domain.Entities;
using TinhLuongService.Domain.Service.Interface;

namespace TinhLuongService.Domain.Service
{
    public class TinhLuongService : ITinhLuongService
    {
        public TinhLuongService()
        {
        }

        public void TinhLuong(KyTinhLuong kyTinhLuong, IList<NhanVienTinhLuong> nhanVienTinhLuongs)
        {
            if (kyTinhLuong == null) throw new ArgumentNullException(nameof(kyTinhLuong));
            if (nhanVienTinhLuongs == null) throw new ArgumentNullException(nameof(nhanVienTinhLuongs));

            foreach (var nhanVienTinhLuong in nhanVienTinhLuongs)
            {
                kyTinhLuong.NhanVienTinhLuongs?.Add(nhanVienTinhLuong);
            }
        }

    }
}