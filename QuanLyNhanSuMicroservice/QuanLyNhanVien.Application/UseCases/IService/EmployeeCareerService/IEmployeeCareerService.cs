
using QuanLyNhanSuMicroservice.QuanLyNhanVien.Domain.Entities;


namespace QuanLyNhanSuMicroservice.QuanLyNhanVien.Application.UseCases.INhanSuServiceClient
{
    public interface IQuanLyCaNhanChucVuService
    {
        void CapNhatChucVu(NhanVien employee, ChucVu position);
        void XoaChucVu(NhanVien employee);
    }
}