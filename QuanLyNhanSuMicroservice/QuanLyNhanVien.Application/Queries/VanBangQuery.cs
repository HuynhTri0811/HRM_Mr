using MediatR;
using QuanLyNhanSuMicroservice.QuanLyNhanVien.Application.DTOs.VanBang;

namespace QuanLyNhanSuMicroservice.QuanLyNhanVien.Application.Queries
{
    public record GetAllVanBangQuery() : IRequest<IEnumerable<VanBangDto>>;
    public record GetVanBangByIdQuery(Guid Id) : IRequest<VanBangDto?>;
    public record GetVanBangByNhanVienQuery(Guid NhanVienId) : IRequest<IEnumerable<VanBangDto>>;
}
