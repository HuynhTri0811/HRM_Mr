using MediatR;
using Microsoft.AspNetCore.Http;
using QuyetDinhService.Domain.Entities;
using QuyetDinhService.QuyetDinhService.Application.DTOs.NangLuong;
using QuyetDinhService.QuyetDinhService.Application.Query;
using QuyetDinhService.QuyetDinhService.Application.Services;
using QuyetDinhService.QuyetDinhService.Domain.Repositories;

namespace QuyetDinhService.QuyetDinhService.Application.Handlers.QuyetDinhNangLuongHandle
{
    public class GetAllQuyetDinhNangLuongHandler(
        IQuyetDinhNangLuongRepositoris repository,
        INhanSuServiceClient nhanSuServiceClient,
        IHttpContextAccessor httpContextAccessor)
        : IRequestHandler<GetAllQuyetDinhNangLuongQuery, IEnumerable<QuyetDinhNangLuongDto>>
    {
        public async Task<IEnumerable<QuyetDinhNangLuongDto>> Handle(GetAllQuyetDinhNangLuongQuery request, CancellationToken cancellationToken)
        {
            var quyetDinhs = await repository.GetAllAsync();
            var token = httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString() ?? string.Empty;

            var tasks = quyetDinhs.Select(async qd =>
            {
                var nhanVien = await nhanSuServiceClient.GetNhanVienByIdAsync(qd.MaNhanVien, token);
                return new QuyetDinhNangLuongDto(
                    qd.Id,
                    qd.SoQuyetDinh,
                    qd.NgayHieuLuc ?? DateTime.MinValue,
                    qd.NoiDung,
                    qd.GhiChu ?? string.Empty,
                    nhanVien!,
                    qd.LuongCoBanCu,
                    qd.LuongCoBanMoi);
            });

            return await Task.WhenAll(tasks);
        }
    }
}
