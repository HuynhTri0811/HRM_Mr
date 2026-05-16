using QuanLyNhanSuMicroservice.QuanLyNhanVien.Domain.Entities.Base;

namespace QuanLyNhanSuMicroservice.QuanLyNhanVien.Domain.Entities
{
    public class NhanVien : ObjectBase
    {
        public string MaNhanVien { get; private set; }
        public string TenNhanVien { get; private set; } = string.Empty;
        public DateTime NgaySinh { get; private set; }
        public string GioiTinh { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;


        public decimal TongLuong => LuongCoBan + PhuCap;
        public decimal LuongCoBan { get; private set; }
        public decimal PhuCap { get; private set; }




        public ChucVu? ChucVu { get; private set; }

        public PhongBan PhongBan { get; private set; }

        public ICollection<VanBang> VanBangs { get; private set; } = new List<VanBang>();

        private NhanVien() { }

        private NhanVien(string MaNhanVien, string TenNhanVien, DateTime NgaySinh, string GioiTinh, string Email, PhongBan PhongBan)
        {
            this.MaNhanVien = MaNhanVien;
            this.TenNhanVien = TenNhanVien;
            this.NgaySinh = NgaySinh;
            this.GioiTinh = GioiTinh;
            this.Email = Email;
            this.PhongBan = PhongBan;
        }

        public static NhanVien Create(string MaNhanVien, string TenNhanVien, DateTime NgaySinh, string GioiTinh, string Email, PhongBan PhongBan)
        {
            if (string.IsNullOrWhiteSpace(MaNhanVien) || string.IsNullOrWhiteSpace(TenNhanVien) || string.IsNullOrWhiteSpace(GioiTinh) || string.IsNullOrWhiteSpace(Email))
                throw new Exception("Thông tin nhân viên không được để trống");
            if (NgaySinh > DateTime.Now)
                throw new Exception("Ngày sinh không được lớn hơn ngày hiện tại");
            if (PhongBan == null)
                throw new Exception("Phòng ban không được để trống");
            return new NhanVien(MaNhanVien, TenNhanVien, NgaySinh, GioiTinh, Email, PhongBan);
        }

        public void UpdateInfo(string TenNhanVien, DateTime NgaySinh, string GioiTinh, string Email, PhongBan PhongBan)
        {

            if (string.IsNullOrWhiteSpace(TenNhanVien) || string.IsNullOrWhiteSpace(GioiTinh) || string.IsNullOrWhiteSpace(Email))
                throw new Exception("Thông tin nhân viên không được để trống");
            if (NgaySinh > DateTime.Now)
                throw new Exception("Ngày sinh không được lớn hơn ngày hiện tại");
            if (PhongBan == null)
                throw new Exception("Phòng ban không được để trống");

            this.TenNhanVien = TenNhanVien;
            this.NgaySinh = NgaySinh;
            this.GioiTinh = GioiTinh;
            this.Email = Email;
            this.PhongBan = PhongBan;
        }





        public void UpdateLuong(decimal LuongCoBan, decimal PhuCap)
        {
            if (LuongCoBan < 0 || PhuCap < 0)
                throw new Exception("Lương và phụ cấp không được nhỏ hơn 0");
            this.LuongCoBan = LuongCoBan;
            this.PhuCap = PhuCap;
        }

        public void CapNhatLuong(decimal luongCoBan)
        {
            if (luongCoBan < 0)
                throw new Exception("Lương không được nhỏ hơn 0");
            this.LuongCoBan = luongCoBan;
        }

        private void UpdateChucVu(ChucVu chucVu)
        {
            if (chucVu == null)
                throw new Exception("Chức vụ không được để trống");
            this.ChucVu = chucVu;
            this.PhuCap = chucVu.PhuCap;
        }

        public void UpdatePosition(ChucVu chucVu)
        {
            if (chucVu == null)
                throw new Exception("Chức vụ không được để trống");


            UpdateChucVu(chucVu);
        }

        internal void DeletePosition()
        {
            this.ChucVu = null;
            this.PhuCap = 0;
        }
    }
}
