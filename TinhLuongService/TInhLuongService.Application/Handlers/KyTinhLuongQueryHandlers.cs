using MediatR;
using TinhLuongService.Application.DTOs;
using TinhLuongService.Application.Query;
using TinhLuongService.Domain.Repositories;

namespace TinhLuongService.Application.Handlers
{
    public class GetAllKyTinhLuongHandler(IKyTinhLuongRepositorie repository) : IRequestHandler<GetAllKyTinhLuongQuery, IEnumerable<KyTinhLuongDto>>
    {
        public async Task<IEnumerable<KyTinhLuongDto>> Handle(GetAllKyTinhLuongQuery request, CancellationToken cancellationToken)
        {
            var list = await repository.GetAllAsync();
            return list.Where(x => x != null).Select(x => new KyTinhLuongDto(x!.Id, x.MaKy, x.NgayBatDau, x.NgayKetThuc, x.ChotTinhLuong, x.UpdatedAt));
        }
    }

    public class GetKyTinhLuongByIdHandler(IKyTinhLuongRepositorie repository) : IRequestHandler<GetKyTinhLuongByIdQuery, KyTinhLuongDto?>
    {
        public async Task<KyTinhLuongDto?> Handle(GetKyTinhLuongByIdQuery request, CancellationToken cancellationToken)
        {
            var x = await repository.GetByIdAsync(request.Id);
            if (x == null) return null;
            return new KyTinhLuongDto(x.Id, x.MaKy, x.NgayBatDau, x.NgayKetThuc, x.ChotTinhLuong, x.UpdatedAt);
        }
    }
}
