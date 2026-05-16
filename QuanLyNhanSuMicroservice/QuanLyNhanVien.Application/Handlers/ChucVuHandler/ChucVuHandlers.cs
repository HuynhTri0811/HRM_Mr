using MediatR;
using QuanLyNhanSuMicroservice.Core.Repositories;
using QuanLyNhanSuMicroservice.QuanLyNhanVien.Application.Command.ChucVu;
using QuanLyNhanSuMicroservice.QuanLyNhanVien.Application.Queries;
using QuanLyNhanSuMicroservice.QuanLyNhanVien.Domain.Entities;

namespace QuanLyNhanSuMicroservice.QuanLyNhanVien.Application.Handlers.ChucVuHandler
{
    public class GetAllChucVuHandler(IChucVuRepository repository) : IRequestHandler<GetAllChucVuQuery, IEnumerable<ChucVuDto>>
    {
        public async Task<IEnumerable<ChucVuDto>> Handle(GetAllChucVuQuery request, CancellationToken cancellationToken)
        {
            var chucVus = await repository.GetAllAsync();
            return chucVus.Select(cv => new ChucVuDto(cv.Id, cv.MaChucVu, cv.TenChucVu, cv.PhuCap));
        }
    }

    public class GetChucVuByIdHandler(IChucVuRepository repository) : IRequestHandler<GetChucVuByIdQuery, ChucVuDto?>
    {
        public async Task<ChucVuDto?> Handle(GetChucVuByIdQuery request, CancellationToken cancellationToken)
        {
            var cv = await repository.GetByIdAsync(request.Id);
            if (cv == null) return null;
            return new ChucVuDto(cv.Id, cv.MaChucVu, cv.TenChucVu, cv.PhuCap);
        }
    }

    public class CreateChucVuHandler(IChucVuRepository repository) : IRequestHandler<CreateChucVuDTO, Guid>
    {
        public async Task<Guid> Handle(CreateChucVuDTO request, CancellationToken cancellationToken)
        {
            var chucVu = ChucVu.Create(request.MaChucVu, request.TenChucVu, request.PhuCap);
            await repository.AddAsync(chucVu);
            await repository.SaveChangesAsync();
            return chucVu.Id;
        }
    }

    public class UpdateChucVuHandler(IChucVuRepository repository) : IRequestHandler<UpdateChucVuDTO, bool>
    {
        public async Task<bool> Handle(UpdateChucVuDTO request, CancellationToken cancellationToken)
        {
            var chucVu = await repository.GetByIdAsync(request.Id);
            if (chucVu == null)
            {
                throw new Exception("Không tìm thấy chức vụ");
            }
            chucVu.Update(request.MaChucVu, request.TenChucVu);
            chucVu.SetPhuCap(request.PhuCap);

            await repository.UpdateAsync(chucVu);
            await repository.SaveChangesAsync();
            return true;
        }
    }

    public class DeleteChucVuHandler(IChucVuRepository repository) : IRequestHandler<DeleteChucVuDTO, bool>
    {
        public async Task<bool> Handle(DeleteChucVuDTO request, CancellationToken cancellationToken)
        {
            var chucVu = await repository.GetByIdAsync(request.Id);
            if (chucVu == null)
            {
                throw new Exception("Không tìm thấy chức vụ");
            }

            await repository.DeleteAsync(chucVu.Id);
            await repository.SaveChangesAsync();
            return true;
        }
    }
}
