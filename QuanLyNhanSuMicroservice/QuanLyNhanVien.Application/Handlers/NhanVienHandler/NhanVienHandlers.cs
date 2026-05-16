using MediatR;
using QuanLyNhanSuMicroservice.Core.Repositories;
using QuanLyNhanSuMicroservice.QuanLyNhanVien.Application.Command.NhanSu;
using QuanLyNhanSuMicroservice.QuanLyNhanVien.Application.Queries;
using QuanLyNhanSuMicroservice.QuanLyNhanVien.Application.Command.PhongBan;
using QuanLyNhanSuMicroservice.QuanLyNhanVien.Application.Command.ChucVu;
using QuanLyNhanSuMicroservice.QuanLyNhanVien.Application.Command.VanBang;
using QuanLyNhanSuMicroservice.QuanLyNhanVien.Domain.Repositories;
using QuanLyNhanSuMicroservice.QuanLyNhanVien.Domain.Entities;

namespace QuanLyNhanSuMicroservice.QuanLyNhanVien.Application.Handlers.NhanVienHandler
{
    public class GetAllEmployeesHandler(INhanVienRepository repository) : IRequestHandler<GetAllEmployeesQuery, IEnumerable<NhanVienDto>>
    {
        public async Task<IEnumerable<NhanVienDto>> Handle(GetAllEmployeesQuery request, CancellationToken cancellationToken)
        {
            var employees = await repository.GetAllAsync();
            return employees.Select(e => new NhanVienDto(
                e.Id,
                e.MaNhanVien,
                e.TenNhanVien,
                e.NgaySinh,
                e.GioiTinh,
                e.Email,
                e.LuongCoBan,
                e.PhuCap,
                e.PhongBan != null ? new PhongBanDto(e.PhongBan.Id, e.PhongBan.MaQuanLy, e.PhongBan.TenPhongBan) : null,
                e.ChucVu != null ? new ChucVuDto(e.ChucVu.Id, e.ChucVu.MaChucVu, e.ChucVu.TenChucVu, e.ChucVu.PhuCap) : null,
                e.VanBangs.Select(v => new VanBangDto(v.Id, v.TenVanBang, v.LoaiVanBang, v.NgayCap, v.NoiCap, e.Id)).ToList()
            ));
        }
    }

    public class GetEmployeeByIdHandler(INhanVienRepository repository) : IRequestHandler<GetEmployeeByIdQuery, NhanVienDto?>
    {
        public async Task<NhanVienDto?> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
        {
            var e = await repository.GetByIdAsync(request.Id);
            if (e == null) return null;
            return new NhanVienDto(
                e.Id,
                e.MaNhanVien,
                e.TenNhanVien,
                e.NgaySinh,
                e.GioiTinh,
                e.Email,
                e.LuongCoBan,
                e.PhuCap,
                e.PhongBan != null ? new PhongBanDto(e.PhongBan.Id, e.PhongBan.MaQuanLy, e.PhongBan.TenPhongBan) : null,
                e.ChucVu != null ? new ChucVuDto(e.ChucVu.Id, e.ChucVu.MaChucVu, e.ChucVu.TenChucVu, e.ChucVu.PhuCap) : null,
                e.VanBangs.Select(v => new VanBangDto(v.Id, v.TenVanBang, v.LoaiVanBang, v.NgayCap, v.NoiCap, e.Id)).ToList()
            );
        }
    }

    public class CreateNhanVienHandler(
        INhanVienRepository repository,
        IPhongBanRepository phongBanRepository,
        IChucVuRepository chucVuRepository
    ) : IRequestHandler<CreateNhanVienDto, Guid>
    {
        public async Task<Guid> Handle(CreateNhanVienDto request, CancellationToken cancellationToken)
        {
            // Kiểm tra email duy nhất
            if (!await repository.IsEmailUniqueAsync(request.Email))
            {
                throw new Exception("Email đã tồn tại trong hệ thống");
            }

            // Kiểm tra Phòng ban tồn tại
            var phongBan = await phongBanRepository.GetByIdAsync(request.PhongBanID);
            if (phongBan == null)
            {
                throw new Exception("Phòng ban không tồn tại");
            }

            // Kiểm tra Chức vụ tồn tại
            var chucVu = await chucVuRepository.GetByIdAsync(request.ChucVuID);
            if (chucVu == null)
            {
                throw new Exception("Chức vụ không tồn tại");
            }

            var employee = NhanVien.Create(
                request.MaNhanVien,
                request.TenNhanVien,
                request.NgaySinh,
                request.GioiTinh,
                request.Email,
                phongBan
            );

            employee.UpdatePosition(chucVu);


            await repository.AddAsync(employee);
            return employee.Id;
        }
    }

    public class UpdateNhanVienHandler(INhanVienRepository repository
        , IPhongBanRepository phongBanRepository
        , IChucVuRepository chucVuRepository
    ) : IRequestHandler<UpdateNhanVienDTO, bool>
    {
        public async Task<bool> Handle(UpdateNhanVienDTO request, CancellationToken cancellationToken)
        {
            var employee = await repository.GetByIdAsync(request.Id);
            if (employee == null)
            {
                throw new Exception("Không tìm thấy nhân viên");
            }
            var phongBan = await phongBanRepository.GetByIdAsync(request.PhongBanID);
            if (phongBan == null)
            {
                throw new Exception("Phòng ban không tồn tại");
            }
            var chucVu = await chucVuRepository.GetByIdAsync(request.ChucVuID);
            if (chucVu == null)
            {
                throw new Exception("Chức vụ không tồn tại");
            }

            employee.UpdateInfo(request.TenNhanVien, request.NgaySinh, request.GioiTinh, request.Email, phongBan);
            employee.UpdatePosition(chucVu);

            await repository.UpdateAsync(employee);
            await repository.SaveChangesAsync();
            return true;
        }
    }

    public class DeleteNhanVienHandler(INhanVienRepository repository) : IRequestHandler<DeleteNhanVienDTO, bool>
    {
        public async Task<bool> Handle(DeleteNhanVienDTO request, CancellationToken cancellationToken)
        {
            var employee = await repository.GetByIdAsync(request.Id);
            if (employee == null)
            {
                throw new Exception("Không tìm thấy nhân viên");
            }

            await repository.DeleteAsync(employee.Id);
            await repository.SaveChangesAsync();
            return true;
        }
    }

    public class UpdateLuongHandler(INhanVienRepository repository) : IRequestHandler<UpdateNhanVienCapNhatLuongDTO, bool>
    {
        public async Task<bool> Handle(UpdateNhanVienCapNhatLuongDTO request, CancellationToken cancellationToken)
        {
            var employee = await repository.GetByIdAsync(request.Id);
            if (employee == null) throw new Exception("Không tìm thấy nhân viên");

            employee.CapNhatLuong(request.LuongCoBan);

            await repository.UpdateAsync(employee);
            await repository.SaveChangesAsync();
            return true;
        }
    }

    public class UpdateChucVuHandler(INhanVienRepository repository, IChucVuRepository chucVuRepository) : IRequestHandler<UpdateNhanVienCapNhatChucVuDTO, bool>
    {
        public async Task<bool> Handle(UpdateNhanVienCapNhatChucVuDTO request, CancellationToken cancellationToken)
        {
            var employee = await repository.GetByIdAsync(request.Id);
            if (employee == null) throw new Exception("Không tìm thấy nhân viên");

            var chucVu = await chucVuRepository.GetByIdAsync(request.ChucVuID);
            if (chucVu == null) throw new Exception("Chức vụ không tồn tại");

            employee.UpdatePosition(chucVu);

            await repository.UpdateAsync(employee);
            await repository.SaveChangesAsync();
            return true;
        }
    }

    public class UpdateNhanVienBoNhiemHandler(INhanVienRepository repository, IChucVuRepository chucVuRepository) : IRequestHandler<UpdateNhanVienBoNhiemDTO, bool>
    {
        public async Task<bool> Handle(UpdateNhanVienBoNhiemDTO request, CancellationToken cancellationToken)
        {
            var employee = await repository.GetByIdAsync(request.Id);
            if (employee == null) throw new Exception("Không tìm thấy nhân viên");

            var chucVu = await chucVuRepository.GetByIdAsync(request.ChucVuMoi);
            if (chucVu == null) throw new Exception("Chức vụ không tồn tại");

            employee.UpdatePosition(chucVu);

            await repository.UpdateAsync(employee);
            await repository.SaveChangesAsync();
            return true;
        }
    }
}
