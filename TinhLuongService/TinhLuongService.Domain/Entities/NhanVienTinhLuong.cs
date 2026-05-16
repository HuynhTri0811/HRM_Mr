namespace TinhLuongService.Domain.Entities
{
    public class NhanVienTinhLuong : ObjectBase
    {
        public Guid NhanVienId { get; private set; }
        public Guid KyTinhLuongId { get; private set; }
        public decimal LuongCoBan { get; private set; }
        public decimal PhuCap { get; private set; }
        public decimal Thuong { get; private set; }
        public decimal Phat { get; private set; }
        public decimal TongLuong { get; private set; }
        public decimal Thue { get; private set; }
        public decimal LuongThucLanh { get; private set; }
        private NhanVienTinhLuong(Guid nhanVienId, Guid kyTinhLuongId, decimal luongCoBan, decimal phuCap, decimal thuong, decimal phat, decimal tongLuong, decimal thue, decimal luongThucLanh)
        {
            NhanVienId = nhanVienId;
            KyTinhLuongId = kyTinhLuongId;
            LuongCoBan = luongCoBan;
            PhuCap = phuCap;
            Thuong = thuong;
            Phat = phat;
            TongLuong = tongLuong;
            Thue = thue;
            LuongThucLanh = luongThucLanh;
        }

        public static NhanVienTinhLuong Create(Guid nhanVienId, Guid kyTinhLuongId, decimal luongCoBan, decimal phuCap, decimal thuong, decimal phat, decimal tongLuong, decimal thue, decimal luongThucLanh)
        {
            if (nhanVienId == Guid.Empty)
                throw new ArgumentException("NhanVienId is required", nameof(nhanVienId));
            if (kyTinhLuongId == Guid.Empty)
                throw new ArgumentException("KyTinhLuongId is required", nameof(kyTinhLuongId));
            if (luongCoBan <= 0)
                throw new ArgumentException("LuongCoBan must be greater than 0", nameof(luongCoBan));
            return new NhanVienTinhLuong(nhanVienId, kyTinhLuongId, luongCoBan, phuCap, thuong, phat, tongLuong, thue, luongThucLanh);
        }
    }
}