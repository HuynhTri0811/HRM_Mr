
using MediatR;
using QuyetDinhService.QuyetDinhService.Application.Services.DTO;
namespace QuyetDinhService.QuyetDinhService.Application.DTOs.NangLuong
{
    public record QuyetDinhNangLuongDto(Guid Id, string SoQuyetDinh, DateTime NgayHieuLuc, string NoiDung, string GhiChu, NhanVienServiceClientDto NhanVien, decimal LuongCu, decimal LuongMoi);

}
