using System;

namespace QuanLyNhanSuMicroservice.QuyetDinhService.Application.Events
{
    public class QuyetDinhBoNhiemCreatedEvent
    {
        public Guid Id { get; set; }
        public string SoQuyetDinh { get; set; } = string.Empty;
        public DateTime NgayQuyetDinh { get; set; }
        public Guid MaNhanVien { get; set; }
        public Guid ChucVuCu { get; set; }
        public Guid ChucVuMoi { get; set; }
        public decimal PhuCapCu { get; set; }
        public decimal PhuCapMoi { get; set; }
    }
}
