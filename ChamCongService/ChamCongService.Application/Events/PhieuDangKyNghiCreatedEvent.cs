using System;

namespace ChamCongService.Application.Events
{
    public class PhieuDangKyNghiCreatedEvent
    {
        public Guid Id { get; set; }
        public DateTime NgayNghi { get; set; }
        public string LyDo { get; set; } = string.Empty;
        public Guid MaNhanVien { get; set; }
        public Guid LoaiChamCongId { get; set; }
    }
}
