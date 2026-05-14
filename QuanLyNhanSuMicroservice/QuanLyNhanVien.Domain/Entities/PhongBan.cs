using System.Security;

namespace QuanLyNhanSuMicroservice.QuanLyNhanVien.Domain.Entities
{
    public class PhongBan : ObjectBase
    {
        public string MaQuanLy { get; private set; }
        public string TenPhongBan { get; private set; }
        private PhongBan()
        {

        }
        private PhongBan(string MaQuanLy, string TenPhongBan)
        {
            this.MaQuanLy = MaQuanLy;
            this.TenPhongBan = TenPhongBan;
        }

        private void Update(string MaQuanLy, string TenPhongBan)
        {
            this.MaQuanLy = MaQuanLy;
            this.TenPhongBan = TenPhongBan;
        }

        public static PhongBan Create(string MaQuanLy, string TenPhongBan)
        {
            if (string.IsNullOrWhiteSpace(MaQuanLy) || string.IsNullOrWhiteSpace(TenPhongBan))
                throw new Exception("Mã quản lý và tên phòng ban không được để trống");
            return new PhongBan(MaQuanLy, TenPhongBan);
        }

        public void CapNhat(string MaQuanLy, string TenPhongBan)
        {
            if (string.IsNullOrWhiteSpace(MaQuanLy) || string.IsNullOrWhiteSpace(TenPhongBan))
                throw new Exception("Mã quản lý và tên phòng ban không được để trống");
            Update(MaQuanLy, TenPhongBan);
        }



    }
}
