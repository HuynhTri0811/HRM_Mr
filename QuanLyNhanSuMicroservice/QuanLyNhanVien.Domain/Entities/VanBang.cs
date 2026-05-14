namespace QuanLyNhanSuMicroservice.QuanLyNhanVien.Domain.Entities
{
    public class VanBang : ObjectBase
    {
        public string TenVanBang { get; private set; }
        public string LoaiVanBang { get; private set; }
        public DateTime NgayCap { get; private set; }
        public string NoiCap { get; private set; }

        public NhanVien NhanVien { get; set; }

        private VanBang()
        {
            TenVanBang = string.Empty;
            LoaiVanBang = string.Empty;
            NoiCap = string.Empty;
        }

        private VanBang(string tenVanBang, string loaiVanBang, DateTime ngayCap, string noiCap, NhanVien nhanVienId)
        {
            TenVanBang = tenVanBang;
            LoaiVanBang = loaiVanBang;
            NgayCap = ngayCap;
            NoiCap = noiCap;
            NhanVien = nhanVienId;
        }

        internal static VanBang Create(string tenVanBang, string loaiVanBang, DateTime ngayCap, string noiCap, NhanVien nhanVienID)
        {
            if (string.IsNullOrEmpty(tenVanBang))
                throw new Exception("Tên văn bằng không được để trống");
            if (string.IsNullOrEmpty(loaiVanBang))
                throw new Exception("Loại văn bằng không được để trống");
            if (nhanVienID == null)
                throw new Exception("Nhân viên không được để trống");

            return new VanBang(tenVanBang, loaiVanBang, ngayCap, noiCap, nhanVienID);
        }

        public void UpdateVanBang(string tenVanBang, string loaiVanBang, DateTime ngayCap, string noiCap)
        {
            if (string.IsNullOrEmpty(tenVanBang))
                throw new Exception("Tên văn bằng không được để trống");
            if (string.IsNullOrEmpty(loaiVanBang))
                throw new Exception("Loại văn bằng không được để trống");


            TenVanBang = tenVanBang;
            LoaiVanBang = loaiVanBang;
            NgayCap = ngayCap;
            NoiCap = noiCap;
        }

        internal Guid GetIdNhanVien()
        {
            if(NhanVien == null)
                return Guid.Empty;
            return NhanVien.Id;
        }
    }
}
