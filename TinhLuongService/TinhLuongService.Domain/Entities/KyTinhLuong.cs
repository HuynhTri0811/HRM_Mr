namespace TinhLuongService.Domain.Entities
{
    public class KyTinhLuong : ObjectBase
    {
        public string MaKy { get; private set; } = string.Empty;
        public DateTime NgayBatDau { get; private set; }
        public DateTime NgayKetThuc { get; private set; }
        public bool ChotTinhLuong { get; private set; } = true;
        public List<NhanVienTinhLuong>? NhanVienTinhLuongs { get; private set; } = new List<NhanVienTinhLuong>();


        private KyTinhLuong(string maKy, DateTime ngayBatDau, DateTime ngayKetThuc)
        {
            MaKy = maKy;
            NgayBatDau = ngayBatDau;
            NgayKetThuc = ngayKetThuc;
        }


        public static KyTinhLuong Create(string maKy, DateTime ngayBatDau, DateTime ngayKetThuc)
        {
            if (ngayBatDau > ngayKetThuc)
            {
                throw new Exception("Ngày bắt đầu phải nhỏ hơn ngày kết thúc");
            }
            if (string.IsNullOrEmpty(maKy))
            {
                throw new Exception("Mã kỳ không được để trống");
            }
            return new KyTinhLuong(maKy, ngayBatDau, ngayKetThuc);
        }

        public void UpdateInfo(string maKy, DateTime ngayBatDau, DateTime ngayKetThuc)
        {
            if (ngayBatDau > ngayKetThuc)
            {
                throw new Exception("Ngày bắt đầu phải nhỏ hơn ngày kết thúc");
            }
            if (string.IsNullOrEmpty(maKy))
            {
                throw new Exception("Mã kỳ không được để trống");
            }
            if (ChotTinhLuong == true)
            {
                throw new Exception("Kỳ đã chốt không thể sửa");
            }
            MaKy = maKy;
            NgayBatDau = ngayBatDau;
            NgayKetThuc = ngayKetThuc;
        }

        public void KhoaKy()
        {
            ChotTinhLuong = true;
        }

        public void MoKy()
        {
            ChotTinhLuong = false;
        }

    }
}