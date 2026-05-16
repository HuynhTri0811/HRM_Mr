using ChamCongService.Domain.Entity.Base;

namespace ChamCongService.Domain.Entity
{
    public class BangChamCongTheoThang : ObjectBase
    {
        public int Thang { get; private set; }
        public int Nam { get; private set; }
        public DateTime TuNgay { get; private set; }
        public DateTime DenNgay { get; private set; }
        public bool IsChot { get; private set; } = false;

        private BangChamCongTheoThang() { }

        private BangChamCongTheoThang(int thang, int nam, DateTime tuNgay, DateTime denNgay)
        {
            Thang = thang;
            Nam = nam;
            TuNgay = tuNgay;
            DenNgay = denNgay;
        }

        public static BangChamCongTheoThang Create(int thang, int nam, DateTime tuNgay, DateTime denNgay)
        {
            if (thang < 1 || thang > 12) throw new Exception("Tháng không hợp lệ");
            if (nam < 2000) throw new Exception("Năm không hợp lệ");
            if (tuNgay >= denNgay) throw new Exception("Từ ngày phải nhỏ hơn Đến ngày");

            return new BangChamCongTheoThang(thang, nam, tuNgay, denNgay);
        }

        public void CapNhat(int thang, int nam, DateTime tuNgay, DateTime denNgay)
        {
            if (IsChot) throw new Exception("Bảng công đã chốt, không thể cập nhật");
            if (thang < 1 || thang > 12) throw new Exception("Tháng không hợp lệ");
            if (nam < 2000) throw new Exception("Năm không hợp lệ");
            if (tuNgay >= denNgay) throw new Exception("Từ ngày phải nhỏ hơn Đến ngày");

            Thang = thang;
            Nam = nam;
            TuNgay = tuNgay;
            DenNgay = denNgay;
        }

        public void ChotBangCong()
        {
            IsChot = true;
        }

        public void MoChotBangCong()
        {
            IsChot = false;
        }
    }
}
