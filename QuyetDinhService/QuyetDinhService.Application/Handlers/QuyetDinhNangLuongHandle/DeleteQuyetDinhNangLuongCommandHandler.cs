using MediatR;
using Microsoft.AspNetCore.Http;
using QuyetDinhService.QuyetDinhService.Application.Commands;
using QuyetDinhService.QuyetDinhService.Application.Services;
using QuyetDinhService.QuyetDinhService.Domain.Repositories;

namespace QuyetDinhService.QuyetDinhService.Application.Handlers.QuyetDinhNangLuongHandle
{
    public class DeleteQuyetDinhNangLuongCommandHandler(
        IQuyetDinhNangLuongRepositoris repository,
        INhanSuServiceClient nhanSuServiceClient,
        IHttpContextAccessor httpContextAccessor)
        : IRequestHandler<DeleteQuyetDinhNangLuongCommand, bool>
    {
        public async Task<bool> Handle(DeleteQuyetDinhNangLuongCommand request, CancellationToken cancellationToken)
        {
            var entity = await repository.GetByIdAsync(request.Id);
            if (entity == null)
            {
                return false;
            }

            await repository.DeleteAsync(request.Id);
            await repository.SaveChangesAsync();

            var token = httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString() ?? string.Empty;
            
            // Khôi phục lại lương cũ
            var isSuccess = await nhanSuServiceClient.UpdateLuongAsync(entity.MaNhanVien, entity.LuongCoBanCu, token);
            if (!isSuccess)
            {
                // Xử lý lỗi nếu cần
            }

            return true;
        }
    }
}
