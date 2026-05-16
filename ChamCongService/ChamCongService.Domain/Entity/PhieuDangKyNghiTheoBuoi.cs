namespace ChamCongService.Domain.Entity
{
    public enum LoaiBuoi
    {
        Sang,
        Chieu
    }

    public class PhieuDangKyNghiTheoBuoi : PhieuDangKyNghi
    {
        public LoaiBuoi LoaiBuoi { get; private set; }

        private PhieuDangKyNghiTheoBuoi() : base() { }

        private PhieuDangKyNghiTheoBuoi(DateTime ngayNghi, string lyDo, Guid maNhanVien, Guid loaiChamCongId, LoaiBuoi loaiBuoi)
            : base(ngayNghi, lyDo, maNhanVien, loaiChamCongId)
        {
            LoaiBuoi = loaiBuoi;
        }

        public static PhieuDangKyNghiTheoBuoi Create(DateTime ngayNghi, string lyDo, Guid maNhanVien, Guid loaiChamCongId, LoaiBuoi loaiBuoi)
        {
            if (string.IsNullOrWhiteSpace(lyDo)) throw new Exception("Lý do không được để trống");
            if (maNhanVien == Guid.Empty) throw new Exception("Mã nhân viên không được để trống");
            if (loaiChamCongId == Guid.Empty) throw new Exception("Loại chấm công không được để trống");
            if (ngayNghi == DateTime.MinValue) throw new Exception("Ngày nghỉ không được để trống");
            if (loaiBuoi != LoaiBuoi.Sang && loaiBuoi != LoaiBuoi.Chieu)
                throw new Exception("Loại buổi không hợp lệ");

            return new PhieuDangKyNghiTheoBuoi(ngayNghi, lyDo, maNhanVien, loaiChamCongId, loaiBuoi);
        }
    }
}
