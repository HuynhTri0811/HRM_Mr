using ChamCongService.Domain.Entity.Base;

namespace ChamCongService.Domain.Entity
{
    public abstract class PhieuDangKyNghi : ObjectBase
    {
        public DateTime NgayNghi { get; protected set; }
        public string LyDo { get; protected set; } = string.Empty;
        public Guid MaNhanVien { get; protected set; }
        public Guid LoaiChamCongId { get; protected set; }
        public LoaiChamCong? LoaiChamCong { get; protected set; }

        protected PhieuDangKyNghi() { }

        protected PhieuDangKyNghi(DateTime ngayNghi, string lyDo, Guid maNhanVien, Guid loaiChamCongId)
        {
            NgayNghi = ngayNghi;
            LyDo = lyDo;
            MaNhanVien = maNhanVien;
            LoaiChamCongId = loaiChamCongId;
        }
    }
}
