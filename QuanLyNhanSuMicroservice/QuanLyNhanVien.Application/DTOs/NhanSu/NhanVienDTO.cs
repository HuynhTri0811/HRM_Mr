using MediatR;
using QuanLyNhanSuMicroservice.QuanLyNhanVien.Application.DTOs.PhongBan;
using QuanLyNhanSuMicroservice.QuanLyNhanVien.Application.DTOs.ChucVu;
using QuanLyNhanSuMicroservice.QuanLyNhanVien.Application.DTOs.VanBang;

namespace QuanLyNhanSuMicroservice.QuanLyNhanVien.Application.DTOs.NhanSu
{
    public record NhanVienDto(Guid Id, string MaNhanVien, string TenNhanVien, DateTime NgaySinh, string GioiTinh, string Email, decimal LuongCoBan, decimal PhuCap, PhongBanDto? PhongBan, ChucVuDto? ChucVu, ICollection<VanBangDto> VanBangs);

    public record CreateNhanVienDto(string MaNhanVien, string TenNhanVien, DateTime NgaySinh, string GioiTinh, string Email, Guid PhongBanID, Guid ChucVuID) : IRequest<Guid>;

    public record UpdateNhanVienRequestDTO(string MaNhanVien, string TenNhanVien, DateTime NgaySinh, string GioiTinh, string Email, Guid PhongBanID, Guid ChucVuID);

    public record UpdateNhanVienDTO(Guid Id, string MaNhanVien, string TenNhanVien, DateTime NgaySinh, string GioiTinh, string Email, Guid PhongBanID, Guid ChucVuID) : IRequest<bool>;

    public record DeleteNhanVienDTO(Guid Id) : IRequest<bool>;

    public record UpdateNhanVienCapNhatLuongRequestDTO(decimal LuongCoBan);
    public record UpdateNhanVienCapNhatLuongDTO(Guid Id, decimal LuongCoBan) : IRequest<bool>;

    public record UpdateNhanVienCapNhatChucVuRequestDTO(Guid ChucVuID);
    public record UpdateNhanVienCapNhatChucVuDTO(Guid Id, Guid ChucVuID) : IRequest<bool>;

    public record UpdateNhanVienBoNhiemRequestDTO(Guid ChucVuMoi, decimal PhuCapMoi);
    public record UpdateNhanVienBoNhiemDTO(Guid Id, Guid ChucVuMoi, decimal PhuCapMoi) : IRequest<bool>;
}
