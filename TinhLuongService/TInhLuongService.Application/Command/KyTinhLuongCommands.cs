using MediatR;

namespace TinhLuongService.Application.Command
{
    public record CreateKyTinhLuongCommand(
        string MaKy,
        DateTime NgayBatDau,
        DateTime NgayKetThuc
    ) : IRequest<Guid>;

    public record UpdateKyTinhLuongCommand(
        Guid Id,
        string MaKy,
        DateTime NgayBatDau,
        DateTime NgayKetThuc,
        DateTime UpdatedAt
    ) : IRequest<bool>;

    public record DeleteKyTinhLuongCommand(Guid Id) : IRequest<bool>;

    public record KhoaKyTinhLuongCommand(Guid Id) : IRequest<bool>;

    public record MoKyTinhLuongCommand(Guid Id) : IRequest<bool>;

    public record TinhLuongCommand(Guid Id) : IRequest<bool>;
}
