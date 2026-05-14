using MediatR;
using QuanLyNhanSuMicroservice.QuanLyNhanVien.Application.DTOs.ChucVu;

namespace QuanLyNhanSuMicroservice.QuanLyNhanVien.Application.Queries
{
    public record GetAllChucVuQuery() : IRequest<IEnumerable<ChucVuDto>>;
    public record GetChucVuByIdQuery(Guid Id) : IRequest<ChucVuDto?>;
}
