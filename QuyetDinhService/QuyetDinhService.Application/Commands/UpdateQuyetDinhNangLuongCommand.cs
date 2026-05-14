using MediatR;

namespace QuyetDinhService.QuyetDinhService.Application.Commands
{
    public record UpdateQuyetDinhNangLuongCommand(
        Guid Id,
        string SoQuyetDinh,
        DateTime NgayQuyetDinh,
        string NoiDung,
        DateTime NgayHieuLuc,
        string? GhiChu,
        decimal LuongCoBanMoi
    ) : IRequest<bool>;
}
