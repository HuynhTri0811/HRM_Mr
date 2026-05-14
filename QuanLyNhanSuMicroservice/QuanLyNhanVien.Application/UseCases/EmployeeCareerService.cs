using QuanLyNhanSuMicroservice.QuanLyNhanVien.Domain.Entities;
using QuanLyNhanSuMicroservice.QuanLyNhanVien.Application.UseCases.INhanSuServiceClient;

namespace QuanLyNhanSuMicroservice.QuanLyNhanVien.Application.UseCases
{
    public class QuanLyCaNhanChucVuService : IQuanLyCaNhanChucVuService
    {
        public void CapNhatChucVu(NhanVien employeeID, ChucVu positionID)
        {
            employeeID.UpdatePosition(positionID);
        }

        public void XoaChucVu(NhanVien employeeID)
        {
            employeeID.DeletePosition();
        }


    }
}
