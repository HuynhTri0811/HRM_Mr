
namespace QuyetDinhService.Domain.Entities
{
    public class QuyetDinhBoNhiem : QuyetDinh
    {
        public Guid MaNhanVien { get; private set; }
        public Guid ChucVuCu { get; private set; }
        public Guid ChucVuMoi { get; private set; }
        public decimal PhuCapCu { get; private set; }
        public decimal PhuCapMoi { get; private set; }
        public string LyDo { get; set; } = string.Empty;

        private QuyetDinhBoNhiem() : base()
        {
            MaNhanVien = Guid.Empty;
            ChucVuCu = Guid.Empty;
            ChucVuMoi = Guid.Empty;
            PhuCapCu = 0;
            PhuCapMoi = 0;
        }

        private QuyetDinhBoNhiem(string soQuyetDinh, DateTime ngayQuyetDinh, string noiDung, DateTime ngayHieuLuc)
            : base(soQuyetDinh, ngayQuyetDinh, noiDung, ngayHieuLuc)
        {
        }

        public static QuyetDinhBoNhiem Create(string soQuyetDinh, DateTime ngayQuyetDinh, string noiDung, DateTime ngayHieuLuc)
        {
            if (string.IsNullOrWhiteSpace(soQuyetDinh))
                throw new Exception("Số quyết định không được để trống");
            if (string.IsNullOrWhiteSpace(noiDung))
                throw new Exception("Nội dung không được để trống");
            return new QuyetDinhBoNhiem(soQuyetDinh, ngayQuyetDinh, noiDung, ngayHieuLuc);
        }

        public void BoNhiem(Guid maNhanVien, Guid chucVuCu, Guid chucVuMoi, decimal phuCapCu, decimal phuCapMoi, string lyDo)
        {
            if (maNhanVien == Guid.Empty)
                throw new Exception("Mã nhân viên không được để trống");
            
            MaNhanVien = maNhanVien;
            ChucVuCu = chucVuCu;
            ChucVuMoi = chucVuMoi;
            PhuCapCu = phuCapCu;
            PhuCapMoi = phuCapMoi;
            LyDo = lyDo;
        }
    }
}
