
using QuyetDinhService.Domain.Entities;

namespace QuyetDinhService.Domain.Entities
{
    public abstract class QuyetDinh : ObjectBase
    {
        public string SoQuyetDinh { get; set; } = string.Empty;
        public DateTime NgayQuyetDinh { get; set; }
        public string NoiDung { get; set; } = string.Empty;
        public DateTime? NgayHieuLuc { get; set; }
        public string? GhiChu { get; set; }

        public QuyetDinh() { }

        public QuyetDinh(string SoQuyetDinh, DateTime NgayQuyetDinh, string NoiDung,DateTime NgayHieuLuc)
        {
            this.SoQuyetDinh = SoQuyetDinh;
            this.NgayQuyetDinh = NgayQuyetDinh;
            this.NoiDung = NoiDung;
            this.NgayHieuLuc = NgayHieuLuc;
        }

        

    }
}
