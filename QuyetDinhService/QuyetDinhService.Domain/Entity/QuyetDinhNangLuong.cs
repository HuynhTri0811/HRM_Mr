
namespace QuyetDinhService.Domain.Entities
{
    public class QuyetDinhNangLuong : QuyetDinh
    {
        public Guid MaNhanVien { get; private set; }

        public decimal LuongCoBanCu { get; private set; }
        public decimal LuongCoBanMoi { get; set; }


        private QuyetDinhNangLuong() : base()
        {
            MaNhanVien = Guid.Empty;
            LuongCoBanCu = 0;
            LuongCoBanMoi = 0;
        }

        private QuyetDinhNangLuong(string soQuyetDinh, DateTime ngayQuyetDinh, string noiDung, DateTime ngayHieuLuc)
            : base(soQuyetDinh, ngayQuyetDinh, noiDung, ngayHieuLuc)
        {
        }

        public static QuyetDinhNangLuong Create(string soQuyetDinh, DateTime ngayQuyetDinh, string noiDung, DateTime ngayHieuLuc)
        {
            if (string.IsNullOrWhiteSpace(soQuyetDinh))
                throw new Exception("Số quyết định không được để trống");
            if (string.IsNullOrWhiteSpace(noiDung))
                throw new Exception("Nội dung không được để trống");
            return new QuyetDinhNangLuong(soQuyetDinh, ngayQuyetDinh, noiDung, ngayHieuLuc);
            
        }

        public void CapNhatLuongCoBan(Guid NhanVien,decimal luongCoBanCu, decimal luongCoBanMoi)
        {
            if(NhanVien == Guid.Empty)
                throw new Exception("Mã nhân viên không được để trống");
            if (luongCoBanCu < 0 || luongCoBanMoi < 0)
                throw new Exception("Lương cơ bản không được nhỏ hơn 0");
            MaNhanVien = NhanVien;
            LuongCoBanCu = luongCoBanCu;
            LuongCoBanMoi = luongCoBanMoi;
        }


    }
}