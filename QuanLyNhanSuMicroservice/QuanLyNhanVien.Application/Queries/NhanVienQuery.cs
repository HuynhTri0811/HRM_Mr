using MediatR;
using QuanLyNhanSuMicroservice.QuanLyNhanVien.Application.DTOs.NhanSu;

namespace QuanLyNhanSuMicroservice.QuanLyNhanVien.Application.Queries
{
    public record GetAllEmployeesQuery() : IRequest<IEnumerable<NhanVienDto>>;
    public record GetEmployeeByIdQuery(Guid Id) : IRequest<NhanVienDto?>;
}
