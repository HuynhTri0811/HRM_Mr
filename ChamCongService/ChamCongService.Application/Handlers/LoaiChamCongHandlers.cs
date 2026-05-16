using MediatR;
using ChamCongService.Application.Commands;
using ChamCongService.Application.DTOs;
using ChamCongService.Application.Query;
using ChamCongService.Domain.Entity;
using ChamCongService.Domain.Repositories;

namespace ChamCongService.Application.Handlers
{
    public class LoaiChamCongHandlers(ILoaiChamCongRepository repository) :
        IRequestHandler<CreateLoaiChamCongCommand, Guid>,
        IRequestHandler<UpdateLoaiChamCongCommand, bool>,
        IRequestHandler<DeleteLoaiChamCongCommand, bool>,
        IRequestHandler<GetAllLoaiChamCongQuery, IEnumerable<LoaiChamCongDto>>,
        IRequestHandler<GetLoaiChamCongByIdQuery, LoaiChamCongDto?>
    {
        public async Task<Guid> Handle(CreateLoaiChamCongCommand request, CancellationToken cancellationToken)
        {
            var entity = LoaiChamCong.Create(request.MaLoai, request.TenLoai, request.HeSo, request.HinhThuc);
            await repository.AddAsync(entity);
            await repository.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<bool> Handle(UpdateLoaiChamCongCommand request, CancellationToken cancellationToken)
        {
            var entity = await repository.GetByIdAsync(request.Id);
            if (entity == null) return false;
            entity.CapNhat(request.MaLoai, request.TenLoai, request.HeSo, request.HinhThuc);
            await repository.UpdateAsync(entity);
            await repository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Handle(DeleteLoaiChamCongCommand request, CancellationToken cancellationToken)
        {
            var entity = await repository.GetByIdAsync(request.Id);
            if (entity == null) return false;
            await repository.DeleteAsync(request.Id);
            await repository.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<LoaiChamCongDto>> Handle(GetAllLoaiChamCongQuery request, CancellationToken cancellationToken)
        {
            var result = await repository.GetAllAsync();
            return result.Where(x => x != null).Select(x => new LoaiChamCongDto(x!.Id, x.MaLoai, x.TenLoai, x.HeSo, x.HinhThuc));
        }

        public async Task<LoaiChamCongDto?> Handle(GetLoaiChamCongByIdQuery request, CancellationToken cancellationToken)
        {
            var x = await repository.GetByIdAsync(request.Id);
            if (x == null) return null;
            return new LoaiChamCongDto(x.Id, x.MaLoai, x.TenLoai, x.HeSo, x.HinhThuc);
        }
    }
}
