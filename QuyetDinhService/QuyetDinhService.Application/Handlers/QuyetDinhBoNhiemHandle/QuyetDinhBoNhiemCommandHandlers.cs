using MediatR;
using Microsoft.AspNetCore.Http;
using QuyetDinhService.Domain.Entities;
using QuyetDinhService.QuyetDinhService.Application.Commands;
using QuyetDinhService.Domain.Repositories;
using QuyetDinhService.QuyetDinhService.Domain.Repositories;
using QuyetDinhService.QuyetDinhService.Application.Services;
using MassTransit;
using QuyetDinhService.QuyetDinhService.Application.Events;

namespace QuyetDinhService.QuyetDinhService.Application.Handlers.QuyetDinhBoNhiemHandle
{
    public class CreateQuyetDinhBoNhiemCommandHandler(
        IQuyetDinhBoNhiemRepository repository,
        INhanSuServiceClient nhanSuServiceClient,
        IHttpContextAccessor httpContextAccessor,
        IPublishEndpoint publishEndpoint)
        : IRequestHandler<CreateQuyetDinhBoNhiemCommand, Guid>
    {
        public async Task<Guid> Handle(CreateQuyetDinhBoNhiemCommand request, CancellationToken cancellationToken)
        {
            var token = httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString() ?? string.Empty;

            // Lấy thông tin nhân viên để lấy phụ cấp cũ
            var nhanVien = await nhanSuServiceClient.GetNhanVienByIdAsync(request.MaNhanVien, token);
            if (nhanVien == null) throw new Exception("Không tìm thấy nhân viên trong hệ thống nhân sự");
            var phuCapCu = nhanVien.PhuCap;

            // Lấy phụ cấp từ NhanSu API cho chức vụ mới
            var chucVuMoi = await nhanSuServiceClient.GetChucVuByIdAsync(request.ChucVuMoi, token);
            if (chucVuMoi == null) throw new Exception("Không tìm thấy chức vụ mới trong hệ thống nhân sự");
            var phuCapMoi = chucVuMoi.PhuCap;

            var entity = QuyetDinhBoNhiem.Create(
                request.SoQuyetDinh,
                request.NgayQuyetDinh,
                request.NoiDung,
                request.NgayHieuLuc
            );

            entity.BoNhiem(request.MaNhanVien, request.ChucVuCu, request.ChucVuMoi, phuCapCu, phuCapMoi, request.LyDo);
            entity.GhiChu = request.GhiChu;

            await repository.AddAsync(entity);
            await repository.SaveChangesAsync();

            // Tạm thời comment out để sử dụng MassTransit Event-Driven
            // await nhanSuServiceClient.UpdateBoNhiemAsync(request.MaNhanVien, request.ChucVuMoi, phuCapMoi, token);

            // Publish integration event to RabbitMQ
            await publishEndpoint.Publish(new QuyetDinhBoNhiemCreatedEvent
            {
                Id = entity.Id,
                SoQuyetDinh = entity.SoQuyetDinh ?? string.Empty,
                NgayQuyetDinh = entity.NgayQuyetDinh,
                MaNhanVien = entity.MaNhanVien,
                ChucVuCu = entity.ChucVuCu,
                ChucVuMoi = entity.ChucVuMoi,
                PhuCapCu = entity.PhuCapCu,
                PhuCapMoi = entity.PhuCapMoi
            }, cancellationToken);

            return entity.Id;
        }
    }

    public class UpdateQuyetDinhBoNhiemCommandHandler(
        IQuyetDinhBoNhiemRepository repository,
        INhanSuServiceClient nhanSuServiceClient,
        IHttpContextAccessor httpContextAccessor)
        : IRequestHandler<UpdateQuyetDinhBoNhiemCommand, bool>
    {
        public async Task<bool> Handle(UpdateQuyetDinhBoNhiemCommand request, CancellationToken cancellationToken)
        {
            var entity = await repository.GetByIdAsync(request.Id);
            if (entity == null) return false;

            var token = httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString() ?? string.Empty;

            // Lấy phụ cấp mới từ NhanSu API
            var chucVuMoi = await nhanSuServiceClient.GetChucVuByIdAsync(request.ChucVuMoi, token);
            if (chucVuMoi == null) throw new Exception("Không tìm thấy chức vụ mới trong hệ thống nhân sự");
            var phuCapMoi = chucVuMoi.PhuCap;

            entity.SoQuyetDinh = request.SoQuyetDinh;
            entity.NgayQuyetDinh = request.NgayQuyetDinh;
            entity.NoiDung = request.NoiDung;
            entity.NgayHieuLuc = request.NgayHieuLuc;
            entity.GhiChu = request.GhiChu;
            entity.LyDo = request.LyDo;

            entity.BoNhiem(entity.MaNhanVien, entity.ChucVuCu, request.ChucVuMoi, entity.PhuCapCu, phuCapMoi, request.LyDo);

            await repository.UpdateAsync(entity);
            await repository.SaveChangesAsync();

            await nhanSuServiceClient.UpdateBoNhiemAsync(entity.MaNhanVien, request.ChucVuMoi, phuCapMoi, token);

            return true;
        }
    }

    public class DeleteQuyetDinhBoNhiemCommandHandler(
        IQuyetDinhBoNhiemRepository repository,
        INhanSuServiceClient nhanSuServiceClient,
        IHttpContextAccessor httpContextAccessor,
        IPublishEndpoint publishEndpoint)
        : IRequestHandler<DeleteQuyetDinhBoNhiemCommand, bool>
    {
        public async Task<bool> Handle(DeleteQuyetDinhBoNhiemCommand request, CancellationToken cancellationToken)
        {
            var entity = await repository.GetByIdAsync(request.Id);
            if (entity == null) return false;

            await repository.DeleteAsync(request.Id);
            await repository.SaveChangesAsync();

            var token = httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString() ?? string.Empty;
            // Khôi phục lại chức vụ cũ và phụ cấp cũ - comment out to use RabbitMQ
            // await nhanSuServiceClient.UpdateBoNhiemAsync(entity.MaNhanVien, entity.ChucVuCu, entity.PhuCapCu, token);

            // Publish integration event to RabbitMQ
            await publishEndpoint.Publish(new QuyetDinhBoNhiemDeletedEvent
            {
                MaNhanVien = entity.MaNhanVien,
                ChucVuCu = entity.ChucVuCu,
                PhuCapCu = entity.PhuCapCu
            }, cancellationToken);

            return true;
        }
    }
}
