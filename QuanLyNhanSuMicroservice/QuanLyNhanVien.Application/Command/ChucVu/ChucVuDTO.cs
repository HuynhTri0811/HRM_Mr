using MediatR;

namespace QuanLyNhanSuMicroservice.QuanLyNhanVien.Application.Command.ChucVu
{
    public record ChucVuDto(Guid Id, string MaChucVu, string TenChucVu, decimal PhuCap);

    public record CreateChucVuDTO(string MaChucVu, string TenChucVu, decimal PhuCap) : IRequest<Guid>;

    public record UpdateChucVuRequestDTO(string MaChucVu, string TenChucVu, decimal PhuCap);

    public record UpdateChucVuDTO(Guid Id, string MaChucVu, string TenChucVu, decimal PhuCap) : IRequest<bool>;

    public record DeleteChucVuDTO(Guid Id) : IRequest<bool>;
}
