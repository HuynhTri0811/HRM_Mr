using MediatR;

namespace QuanLyNhanSuMicroservice.QuanLyNhanVien.Application.Command.PhongBan
{
    public record CreatePhongBanDTO(string MaPhongBan, string TenPhongBan) : IRequest<Guid>;

    public record PhongBanDto(Guid Id, string MaPhongBan, string TenPhongBan);
    public record DeletePhongBanDTo(Guid Id) : IRequest<bool>;

    public record UpdatePhongBanRequestDTO(string MaPhongBan, string TenPhongBan);
    public record UpdatePhongBanDTO(Guid Id, string MaPhongBan, string TenPhongBan) : IRequest<bool>;
}
