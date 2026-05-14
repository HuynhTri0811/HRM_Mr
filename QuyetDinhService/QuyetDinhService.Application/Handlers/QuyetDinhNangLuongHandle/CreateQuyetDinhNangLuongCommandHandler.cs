using MediatR;
using Microsoft.AspNetCore.Http;
using QuyetDinhService.Domain.Entities;
using QuyetDinhService.QuyetDinhService.Application.Commands;
using QuyetDinhService.QuyetDinhService.Application.Services;
using QuyetDinhService.QuyetDinhService.Domain.Repositories;

namespace QuyetDinhService.QuyetDinhService.Application.Handlers.QuyetDinhNangLuongHandle
{
    public class CreateQuyetDinhNangLuongCommandHandler(
        IQuyetDinhNangLuongRepositoris repository,
        INhanSuServiceClient nhanSuServiceClient,
        IHttpContextAccessor httpContextAccessor)
        : IRequestHandler<CreateQuyetDinhNangLuongCommand, Guid>
    {
        public async Task<Guid> Handle(CreateQuyetDinhNangLuongCommand request, CancellationToken cancellationToken)
        {
            var entity = QuyetDinhNangLuong.Create(
                request.SoQuyetDinh,
                request.NgayQuyetDinh,
                request.NoiDung,
                request.NgayHieuLuc
            );

            entity.CapNhatLuongCoBan(request.MaNhanVien, request.LuongCoBanCu, request.LuongCoBanMoi);
            entity.GhiChu = request.GhiChu;

            await repository.AddAsync(entity);
            await repository.SaveChangesAsync();

            var token = httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString() ?? string.Empty;
            
            // Cập nhật lương sang QuanLyNhanVienService
            var isSuccess = await nhanSuServiceClient.UpdateLuongAsync(request.MaNhanVien, request.LuongCoBanMoi, token);
            if (!isSuccess)
            {
                // Tùy theo yêu cầu, có thể throw exception hoặc chỉ log lỗi (hiện client đã log)
                // throw new Exception("Không thể cập nhật lương mới cho nhân viên.");
            }

            return entity.Id;
        }
    }
}
