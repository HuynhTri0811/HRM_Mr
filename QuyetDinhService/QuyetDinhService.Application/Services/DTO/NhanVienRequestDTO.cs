namespace QuyetDinhService.QuyetDinhService.Application.Services.DTO
{
    public class NhanVienRequestDTO
    {
        public Guid MaNhanVien { get; private set; }
        public string MaQuanLy { get; private set; }
        public string HoTen { get; private set; }
        public string PhongBan { get; private set; }
        public decimal LuongCoBan { get; private set; }
        public NhanVienRequestDTO(Guid maNhanVien, string maQuanLy, string hoTen, string phongBan, decimal luongCoBan)
        {
            MaNhanVien = maNhanVien;
            MaQuanLy = maQuanLy;
            HoTen = hoTen;
            PhongBan = phongBan;
            LuongCoBan = luongCoBan;
        }
    }
}