using MediatR;
using QuanLyNhanSuMicroservice.QuanLyNhanVien.Application.Command.NhanSu;

namespace QuanLyNhanSuMicroservice.QuanLyNhanVien.Application.Queries
{
    public record GetAllEmployeesQuery(int PageNumber = 1, int PageSize = 10) : IRequest<IEnumerable<NhanVienDto>>;
    public record GetEmployeeByIdQuery(Guid Id) : IRequest<NhanVienDto?>;
}
