using ChamCongService.Domain.Entity.Base;

namespace ChamCongService.Domain.Entity
{
    public enum HinhThucNghi
    {
        NghiTheoGio,
        NghiTheoBuoi,
        NghiTheoNgay
    }

    public class LoaiChamCong : ObjectBase
    {
        public string MaLoai { get; private set; } = string.Empty;
        public string TenLoai { get; private set; } = string.Empty;
        public double HeSo { get; private set; }
        public HinhThucNghi HinhThuc { get; private set; }

        private LoaiChamCong() { }

        private LoaiChamCong(string maLoai, string tenLoai, double heSo, HinhThucNghi hinhThuc)
        {
            MaLoai = maLoai;
            TenLoai = tenLoai;
            HeSo = heSo;
            HinhThuc = hinhThuc;
        }

        public static LoaiChamCong Create(string maLoai, string tenLoai, double heSo, HinhThucNghi hinhThuc)
        {
            if (string.IsNullOrWhiteSpace(maLoai)) throw new Exception("Mã loại không được để trống");
            if (string.IsNullOrWhiteSpace(tenLoai)) throw new Exception("Tên loại không được để trống");
            return new LoaiChamCong(maLoai, tenLoai, heSo, hinhThuc);
        }

        public void CapNhat(string maLoai, string tenLoai, double heSo, HinhThucNghi hinhThuc)
        {
            if (string.IsNullOrWhiteSpace(maLoai)) throw new Exception("Mã loại không được để trống");
            if (string.IsNullOrWhiteSpace(tenLoai)) throw new Exception("Tên loại không được để trống");
            MaLoai = maLoai;
            TenLoai = tenLoai;
            HeSo = heSo;
            HinhThuc = hinhThuc;
        }
    }
}
