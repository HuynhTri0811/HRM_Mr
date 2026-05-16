namespace ChamCongService.Domain.Entity
{
    public class PhieuDangKyNghiTheoNgay : PhieuDangKyNghi
    {
        private PhieuDangKyNghiTheoNgay() : base() { }

        private PhieuDangKyNghiTheoNgay(DateTime ngayNghi, string lyDo, Guid maNhanVien, Guid loaiChamCongId)
            : base(ngayNghi, lyDo, maNhanVien, loaiChamCongId)
        {
        }

        public static PhieuDangKyNghiTheoNgay Create(DateTime ngayNghi, string lyDo, Guid maNhanVien, Guid loaiChamCongId)
        {
            if (string.IsNullOrWhiteSpace(lyDo)) throw new Exception("Lý do không được để trống");
            if (maNhanVien == Guid.Empty) throw new Exception("Mã nhân viên không được để trống");
            if (loaiChamCongId == Guid.Empty) throw new Exception("Loại chấm công không được để trống");
            if (ngayNghi == DateTime.MinValue) throw new Exception("Ngày nghỉ không được để trống");

            return new PhieuDangKyNghiTheoNgay(ngayNghi, lyDo, maNhanVien, loaiChamCongId);
        }
    }
}
