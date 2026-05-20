using MediatR;
using Microsoft.AspNetCore.Http;
using QuyetDinhService.QuyetDinhService.Application.Commands;
using QuyetDinhService.QuyetDinhService.Application.Services;
using QuyetDinhService.QuyetDinhService.Domain.Repositories;

namespace QuyetDinhService.QuyetDinhService.Application.Handlers.QuyetDinhNangLuongHandle
{
    public class UpdateQuyetDinhNangLuongCommandHandler(
        IQuyetDinhNangLuongRepositoris repository,
        INhanSuServiceClient nhanSuServiceClient,
        IHttpContextAccessor httpContextAccessor)
        : IRequestHandler<UpdateQuyetDinhNangLuongCommand, bool>
    {
        public async Task<bool> Handle(UpdateQuyetDinhNangLuongCommand request, CancellationToken cancellationToken)
        {
            var entity = await repository.GetByIdAsync(request.Id);
            if (entity == null)
            {
                return false;
            }

            entity.SoQuyetDinh = request.SoQuyetDinh;
            entity.NgayQuyetDinh = request.NgayQuyetDinh;
            entity.NoiDung = request.NoiDung;
            entity.NgayHieuLuc = request.NgayHieuLuc;
            entity.GhiChu = request.GhiChu;
            
            entity.CapNhatLuongCoBan(entity.MaNhanVien, entity.LuongCoBanCu, request.LuongCoBanMoi);

            await repository.UpdateAsync(entity, request.UpdatedAt);
            await repository.SaveChangesAsync();

            var token = httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString() ?? string.Empty;
            
            // Cập nhật lương sang QuanLyNhanVienService
            var isSuccess = await nhanSuServiceClient.UpdateLuongAsync(entity.MaNhanVien, request.LuongCoBanMoi, token);
            if (!isSuccess)
            {
                // Xử lý lỗi nếu cần
            }

            return true;
        }
    }
}
