using MediatR;
using QuanLyNhanSuMicroservice.QuanLyNhanVien.Application.DTOs.NhanSu;

namespace QuanLyNhanSuMicroservice.QuanLyNhanVien.Application.DTOs.VanBang
{
    public record VanBangDto(Guid Id, string TenVanBang, string LoaiVanBang, DateTime NgayCap, string NoiCap, Guid NhanVienID);

    public record CreateVanBangDto(string TenVanBang, string LoaiVanBang, DateTime NgayCap, string NoiCap, Guid NhanVienID) : IRequest<VanBangDto>;

    public record UpdateVanBangRequestDTO(string TenVanBang, string LoaiVanBang, DateTime NgayCap, string NoiCap, Guid NhanVienID);

    public record UpdateVanBangDTO(Guid Id, string TenVanBang, string LoaiVanBang, DateTime NgayCap, string NoiCap, Guid NhanVienID) : IRequest<VanBangDto?>;
    public record DeleteVanBangDTO(Guid Id) : IRequest<bool>;
}
