using System;

namespace QuanLyNhanSuMicroservice.QuanLyNhanVien.Application.Events
{
    public class NhanVienCreatedEvent
    {
        public Guid Id { get; set; }
        public string MaNhanVien { get; set; } = string.Empty;
        public string TenNhanVien { get; set; } = string.Empty;
        public DateTime NgaySinh { get; set; }
        public string GioiTinh { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public decimal LuongCoBan { get; set; }
        public decimal PhuCap { get; set; }
    }
}
