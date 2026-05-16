using MediatR;
using QuanLyNhanSuMicroservice.Core.Repositories;
using QuanLyNhanSuMicroservice.QuanLyNhanVien.Application.Command.PhongBan;
using QuanLyNhanSuMicroservice.QuanLyNhanVien.Application.Queries;
using QuanLyNhanSuMicroservice.QuanLyNhanVien.Domain.Entities;
using QuanLyNhanSuMicroservice.QuanLyNhanVien.Domain.Repositories;


namespace QuanLyNhanSuMicroservice.QuanLyNhanVien.Application.Handlers.PhongBanHandler
{
    public class GetAllPhongBanHandler(IPhongBanRepository repository) : IRequestHandler<GetAllPhongBanQuery, IEnumerable<PhongBanDto>>
    {
        public async Task<IEnumerable<PhongBanDto>> Handle(GetAllPhongBanQuery request, CancellationToken cancellationToken)
        {
            var phongBans = await repository.GetAllAsync();
            return phongBans.Select(pb => new PhongBanDto(pb.Id, pb.MaQuanLy, pb.TenPhongBan));
        }

    }


    public class CreatePhongBanHandler(IPhongBanRepository repository) : IRequestHandler<CreatePhongBanDTO, Guid>
    {
        public async Task<Guid> Handle(CreatePhongBanDTO request, CancellationToken cancellationToken)
        {
            var phongBan = PhongBan.Create(request.MaPhongBan, request.TenPhongBan);
            // 3. Lưu vào Database thông qua Repository (DIP)
            await repository.AddAsync(phongBan);

            return phongBan.Id;
        }

    }

    public class DeletePhongBanHandler(IPhongBanRepository repository) : IRequestHandler<DeletePhongBanDTo, bool>
    {
        public async Task<bool> Handle(DeletePhongBanDTo request, CancellationToken cancellationToken)
        {
            var phongBan = await repository.GetByIdAsync(request.Id);
            if (phongBan == null)
            {
                throw new Exception("Không tìm thấy phòng ban");
            }
            await repository.DeleteAsync(phongBan.Id);
            await repository.SaveChangesAsync();
            return true;
        }
    }

    public class UpdatePhongBanHandler(IPhongBanRepository repository) : IRequestHandler<UpdatePhongBanDTO, bool>
    {
        public async Task<bool> Handle(UpdatePhongBanDTO request, CancellationToken cancellationToken)
        {
            var phongBan = await repository.GetByIdAsync(request.Id);
            if (phongBan == null)
            {
                throw new Exception("Không tìm thấy phòng ban");
            }
            phongBan.CapNhat(request.MaPhongBan, request.TenPhongBan);

            await repository.UpdateAsync(phongBan);
            await repository.SaveChangesAsync();

            return true;
        }
    }
}