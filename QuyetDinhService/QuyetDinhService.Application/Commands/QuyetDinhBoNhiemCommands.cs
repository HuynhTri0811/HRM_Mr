using MediatR;

namespace QuyetDinhService.QuyetDinhService.Application.Commands
{
    public record CreateQuyetDinhBoNhiemCommand(
        string SoQuyetDinh,
        DateTime NgayQuyetDinh,
        string NoiDung,
        DateTime NgayHieuLuc,
        string? GhiChu,
        Guid MaNhanVien,
        Guid ChucVuCu,
        Guid ChucVuMoi,
        string LyDo
    ) : IRequest<Guid>;

    public record UpdateQuyetDinhBoNhiemCommand(
        Guid Id,
        string SoQuyetDinh,
        DateTime NgayQuyetDinh,
        string NoiDung,
        DateTime NgayHieuLuc,
        string? GhiChu,
        Guid ChucVuMoi,
        string LyDo,
        DateTime UpdatedAt
    ) : IRequest<bool>;

    public record DeleteQuyetDinhBoNhiemCommand(Guid Id) : IRequest<bool>;
}
