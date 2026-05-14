using MediatR;

namespace QuyetDinhService.QuyetDinhService.Application.Commands
{
    public record CreateQuyetDinhNangLuongCommand(
        string SoQuyetDinh,
        DateTime NgayQuyetDinh,
        string NoiDung,
        DateTime NgayHieuLuc,
        string? GhiChu,
        Guid MaNhanVien,
        decimal LuongCoBanCu,
        decimal LuongCoBanMoi
    ) : IRequest<Guid>;
}
