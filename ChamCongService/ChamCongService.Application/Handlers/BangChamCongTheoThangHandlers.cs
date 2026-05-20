using MediatR;
using ChamCongService.Application.Commands;
using ChamCongService.Application.DTOs;
using ChamCongService.Application.Query;
using ChamCongService.Domain.Entity;
using ChamCongService.Domain.Repositories;

namespace ChamCongService.Application.Handlers
{
    public class BangChamCongTheoThangHandlers(IBangChamCongTheoThangRepository repository) :
        IRequestHandler<CreateBangChamCongTheoThangCommand, Guid>,
        IRequestHandler<UpdateBangChamCongTheoThangCommand, bool>,
        IRequestHandler<DeleteBangChamCongTheoThangCommand, bool>,
        IRequestHandler<ChotBangChamCongCommand, bool>,
        IRequestHandler<MoChotBangChamCongCommand, bool>,
        IRequestHandler<GetAllBangChamCongTheoThangQuery, IEnumerable<BangChamCongTheoThangDto>>,
        IRequestHandler<GetBangChamCongTheoThangByIdQuery, BangChamCongTheoThangDto?>
    {
        public async Task<Guid> Handle(CreateBangChamCongTheoThangCommand request, CancellationToken cancellationToken)
        {
            var entity = BangChamCongTheoThang.Create(request.Thang, request.Nam, request.TuNgay, request.DenNgay);
            await repository.AddAsync(entity);
            await repository.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<bool> Handle(UpdateBangChamCongTheoThangCommand request, CancellationToken cancellationToken)
        {
            var entity = await repository.GetByIdAsync(request.Id);
            if (entity == null) return false;
            entity.CapNhat(request.Thang, request.Nam, request.TuNgay, request.DenNgay);
            await repository.UpdateAsync(entity, request.UpdatedAt);
            await repository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Handle(DeleteBangChamCongTheoThangCommand request, CancellationToken cancellationToken)
        {
            var entity = await repository.GetByIdAsync(request.Id);
            if (entity == null) return false;
            await repository.DeleteAsync(request.Id);
            await repository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Handle(ChotBangChamCongCommand request, CancellationToken cancellationToken)
        {
            var entity = await repository.GetByIdAsync(request.Id);
            if (entity == null) return false;
            entity.ChotBangCong();
            await repository.UpdateAsync(entity, request.UpdatedAt);
            await repository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Handle(MoChotBangChamCongCommand request, CancellationToken cancellationToken)
        {
            var entity = await repository.GetByIdAsync(request.Id);
            if (entity == null) return false;
            entity.MoChotBangCong();
            await repository.UpdateAsync(entity, request.UpdatedAt);
            await repository.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<BangChamCongTheoThangDto>> Handle(GetAllBangChamCongTheoThangQuery request, CancellationToken cancellationToken)
        {
            var result = await repository.GetAllAsync();
            return result.Where(x => x != null).Select(x => new BangChamCongTheoThangDto(x!.Id, x.Thang, x.Nam, x.TuNgay, x.DenNgay, x.IsChot, x.UpdatedAt));
        }

        public async Task<BangChamCongTheoThangDto?> Handle(GetBangChamCongTheoThangByIdQuery request, CancellationToken cancellationToken)
        {
            var x = await repository.GetByIdAsync(request.Id);
            if (x == null) return null;
            return new BangChamCongTheoThangDto(x.Id, x.Thang, x.Nam, x.TuNgay, x.DenNgay, x.IsChot, x.UpdatedAt);
        }
    }
}
