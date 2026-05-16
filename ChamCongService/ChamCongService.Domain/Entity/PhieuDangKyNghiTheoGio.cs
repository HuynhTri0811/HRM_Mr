namespace ChamCongService.Domain.Entity
{
    public class PhieuDangKyNghiTheoGio : PhieuDangKyNghi
    {
        public TimeSpan TuGio { get; private set; }
        public TimeSpan DenGio { get; private set; }

        private PhieuDangKyNghiTheoGio() : base() { }

        private PhieuDangKyNghiTheoGio(DateTime ngayNghi, string lyDo, Guid maNhanVien, Guid loaiChamCongId, TimeSpan tuGio, TimeSpan denGio)
            : base(ngayNghi, lyDo, maNhanVien, loaiChamCongId)
        {
            TuGio = tuGio;
            DenGio = denGio;
        }

        public static PhieuDangKyNghiTheoGio Create(DateTime ngayNghi, string lyDo, Guid maNhanVien, Guid loaiChamCongId, TimeSpan tuGio, TimeSpan denGio)
        {
            if (tuGio > TimeSpan.Zero && denGio > TimeSpan.Zero)
            {
                if (tuGio >= denGio) throw new Exception("Giờ bắt đầu phải nhỏ hơn giờ kết thúc");
            }
            if (string.IsNullOrWhiteSpace(lyDo)) throw new Exception("Lý do không được để trống");
            if (maNhanVien == Guid.Empty) throw new Exception("Mã nhân viên không được để trống");
            if (loaiChamCongId == Guid.Empty) throw new Exception("Loại chấm công không được để trống");
            if (ngayNghi == DateTime.MinValue) throw new Exception("Ngày nghỉ không được để trống");

            return new PhieuDangKyNghiTheoGio(ngayNghi, lyDo, maNhanVien, loaiChamCongId, tuGio, denGio);
        }
    }
}
