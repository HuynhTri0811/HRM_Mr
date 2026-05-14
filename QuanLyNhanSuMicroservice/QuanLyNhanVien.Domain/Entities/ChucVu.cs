namespace QuanLyNhanSuMicroservice.QuanLyNhanVien.Domain.Entities
{
    public class ChucVu : ObjectBase
    {
        public string MaChucVu { get; private set; }
        public string TenChucVu { get; private set; }
        public decimal PhuCap { get; private set; } = 0;

        private ChucVu()
        {
            MaChucVu = string.Empty;
            TenChucVu = string.Empty;
        }

        private ChucVu(string maChucVu, string tenChucVu, decimal phuCap)
        {
            MaChucVu = maChucVu;
            TenChucVu = tenChucVu;
            PhuCap = phuCap;
        }

        public static ChucVu Create(string maChucVu, string tenChucVu, decimal phuCap)
        {
            if (string.IsNullOrWhiteSpace(maChucVu) || string.IsNullOrWhiteSpace(tenChucVu))
                throw new Exception("Mã quản lý và tên chức vụ không được để trống");
            if (phuCap < 0)
                throw new Exception("Tiền thưởng chức vụ không được < 0");
            return new ChucVu(maChucVu, tenChucVu, phuCap);
        }

        public  void Update(string maChucVu, string tenChucVu)
        {
            if (string.IsNullOrWhiteSpace(maChucVu))
                throw new Exception("Mã quản lý không được để trống");
            if(string .IsNullOrWhiteSpace(tenChucVu))
                throw new Exception("Tên chức vụ không được để trống");

            MaChucVu = maChucVu;
            TenChucVu = tenChucVu;
        }

        public void SetPhuCap(decimal phuCap)
        {
            if (phuCap < 0)
                throw new Exception("Tiền thưởng chức vụ không được < 0");
            PhuCap = phuCap;
        }

    }
}
