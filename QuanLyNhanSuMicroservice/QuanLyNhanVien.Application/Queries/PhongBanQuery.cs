using MediatR;
using QuanLyNhanSuMicroservice.QuanLyNhanVien.Application.Command.PhongBan;

namespace QuanLyNhanSuMicroservice.QuanLyNhanVien.Application.Queries
{
    public record GetAllPhongBanQuery() : IRequest<IEnumerable<PhongBanDto>>;
    public record CreatePhongBanQuery() : IRequest<IEnumerable<CreatePhongBanDTO>>;
}




