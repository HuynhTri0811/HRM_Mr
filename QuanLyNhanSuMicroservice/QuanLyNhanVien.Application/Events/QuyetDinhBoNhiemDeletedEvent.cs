using System;

namespace QuyetDinhService.QuyetDinhService.Application.Events
{
    public class QuyetDinhBoNhiemDeletedEvent
    {
        public Guid MaNhanVien { get; set; }
        public Guid ChucVuCu { get; set; }
        public decimal PhuCapCu { get; set; }
    }
}
