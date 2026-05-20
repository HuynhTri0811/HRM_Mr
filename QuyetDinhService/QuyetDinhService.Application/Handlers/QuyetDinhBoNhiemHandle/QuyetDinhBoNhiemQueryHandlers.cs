using MediatR;
using Microsoft.AspNetCore.Http;
using QuyetDinhService.QuyetDinhService.Application.DTOs.BoNhiem;
using QuyetDinhService.QuyetDinhService.Application.Query;
using QuyetDinhService.QuyetDinhService.Application.Services;
using QuyetDinhService.QuyetDinhService.Domain.Repositories;

namespace QuyetDinhService.QuyetDinhService.Application.Handlers.QuyetDinhBoNhiemHandle
{
    public class GetAllQuyetDinhBoNhiemHandler(
        IQuyetDinhBoNhiemRepository repository,
        INhanSuServiceClient nhanSuServiceClient,
        IHttpContextAccessor httpContextAccessor)
        : IRequestHandler<GetAllQuyetDinhBoNhiemQuery, IEnumerable<QuyetDinhBoNhiemDto>>
    {
        public async Task<IEnumerable<QuyetDinhBoNhiemDto>> Handle(GetAllQuyetDinhBoNhiemQuery request, CancellationToken cancellationToken)
        {
            var quyetDinhs = await repository.GetAllAsync();
            var token = httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString() ?? string.Empty;

            var tasks = quyetDinhs.Select(async qd =>
            {
                var nhanVien = await nhanSuServiceClient.GetNhanVienByIdAsync(qd.MaNhanVien, token);
                return new QuyetDinhBoNhiemDto(
                    qd.Id,
                    qd.SoQuyetDinh,
                    qd.NgayHieuLuc ?? DateTime.MinValue,
                    qd.NoiDung,
                    qd.GhiChu ?? string.Empty,
                    nhanVien!,
                    qd.ChucVuCu,
                    qd.ChucVuMoi,
                    qd.PhuCapCu,
                    qd.PhuCapMoi,
                    qd.LyDo,
                    qd.UpdatedAt);
            });

            return await Task.WhenAll(tasks);
        }
    }

    public class GetQuyetDinhBoNhiemByIdHandler(
        IQuyetDinhBoNhiemRepository repository,
        INhanSuServiceClient nhanSuServiceClient,
        IHttpContextAccessor httpContextAccessor)
        : IRequestHandler<GetQuyetDinhBoNhiemByIdQuery, QuyetDinhBoNhiemDto?>
    {
        public async Task<QuyetDinhBoNhiemDto?> Handle(GetQuyetDinhBoNhiemByIdQuery request, CancellationToken cancellationToken)
        {
            var qd = await repository.GetByIdAsync(request.Id);
            if (qd == null) return null;

            var token = httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString() ?? string.Empty;
            var nhanVien = await nhanSuServiceClient.GetNhanVienByIdAsync(qd.MaNhanVien, token);

            return new QuyetDinhBoNhiemDto(
                qd.Id,
                qd.SoQuyetDinh,
                qd.NgayHieuLuc ?? DateTime.MinValue,
                qd.NoiDung,
                qd.GhiChu ?? string.Empty,
                nhanVien!,
                qd.ChucVuCu,
                qd.ChucVuMoi,
                qd.PhuCapCu,
                qd.PhuCapMoi,
                qd.LyDo,
                qd.UpdatedAt);
        }
    }
}
