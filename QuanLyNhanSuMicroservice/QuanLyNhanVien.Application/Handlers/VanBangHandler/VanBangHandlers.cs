using MediatR;
using QuanLyNhanSuMicroservice.Core.Repositories;
using QuanLyNhanSuMicroservice.QuanLyNhanVien.Application.Command.VanBang;
using QuanLyNhanSuMicroservice.QuanLyNhanVien.Application.Queries;
using QuanLyNhanSuMicroservice.QuanLyNhanVien.Domain.Entities;
using QuanLyNhanSuMicroservice.QuanLyNhanVien.Domain.Repositories;


namespace QuanLyNhanSuMicroservice.QuanLyNhanVien.Application.Handlers.VanBangHandler
{
    public class GetAllVanBangHandler(IVanBangRepository repository) : IRequestHandler<GetAllVanBangQuery, IEnumerable<VanBangDto>>
    {
        public async Task<IEnumerable<VanBangDto>> Handle(GetAllVanBangQuery request, CancellationToken cancellationToken)
        {
            var list = await repository.GetAllAsync();
            return list.Select(x => new VanBangDto(x.Id, x.TenVanBang, x.LoaiVanBang, x.NgayCap, x.NoiCap, x.GetIdNhanVien()));
        }
    }

    public class GetVanBangByIdHandler(IVanBangRepository repository) : IRequestHandler<GetVanBangByIdQuery, VanBangDto?>
    {
        public async Task<VanBangDto?> Handle(GetVanBangByIdQuery request, CancellationToken cancellationToken)
        {
            var x = await repository.GetByIdAsync(request.Id);
            if (x == null) return null;
            return new VanBangDto(x.Id, x.TenVanBang, x.LoaiVanBang, x.NgayCap, x.NoiCap, x.GetIdNhanVien());
        }
    }

    public class GetVanBangByNhanVienHandler(IVanBangRepository repository) : IRequestHandler<GetVanBangByNhanVienQuery, IEnumerable<VanBangDto>>
    {
        public async Task<IEnumerable<VanBangDto>> Handle(GetVanBangByNhanVienQuery request, CancellationToken cancellationToken)
        {
            var list = await repository.GetByNhanVienIdAsync(request.NhanVienId);
            return list.Select(x => new VanBangDto(x.Id, x.TenVanBang, x.LoaiVanBang, x.NgayCap, x.NoiCap, x.GetIdNhanVien()));
        }
    }

    public class CreateVanBangHandler(IVanBangRepository repository, INhanVienRepository nhanVienRepository) : IRequestHandler<CreateVanBangDto, VanBangDto>
    {
        public async Task<VanBangDto> Handle(CreateVanBangDto request, CancellationToken cancellationToken)
        {
            var nhanVien = await nhanVienRepository.GetByIdAsync(request.NhanVienID);
            if (nhanVien == null) throw new Exception("Nhân viên không tồn tại");

            var entity = VanBang.Create(request.TenVanBang, request.LoaiVanBang, request.NgayCap, request.NoiCap, nhanVien);
            await repository.AddAsync(entity);
            await repository.SaveChangesAsync();


            return new VanBangDto(entity.Id, entity.TenVanBang, entity.LoaiVanBang, entity.NgayCap, entity.NoiCap, entity.GetIdNhanVien());
        }
    }

    public class UpdateVanBangHandler(IVanBangRepository repository) : IRequestHandler<UpdateVanBangDTO, VanBangDto>
    {
        public async Task<VanBangDto> Handle(UpdateVanBangDTO request, CancellationToken cancellationToken)
        {
            var entity = await repository.GetByIdAsync(request.Id);
            if (entity == null) throw new Exception("Không tìm thấy văn bằng");

            entity.UpdateVanBang(request.TenVanBang, request.LoaiVanBang, request.NgayCap, request.NoiCap);

            await repository.UpdateAsync(entity);
            await repository.SaveChangesAsync();
            return new VanBangDto(entity.Id, entity.TenVanBang, entity.LoaiVanBang, entity.NgayCap, entity.NoiCap, entity.GetIdNhanVien());
        }
    }

    public class DeleteVanBangHandler(IVanBangRepository repository) : IRequestHandler<DeleteVanBangDTO, bool>
    {
        public async Task<bool> Handle(DeleteVanBangDTO request, CancellationToken cancellationToken)
        {
            await repository.DeleteAsync(request.Id);
            return true;
        }
    }
}
