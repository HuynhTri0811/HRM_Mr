using ChamCongService.Application.DTOs;
using ChamCongService.Domain.Entity;

namespace ChamCongService.Application.Mapping
{
    public static class PhieuDangKyNghiDtoMapper
    {
        public static PhieuDangKyNghiDto MapToDto(PhieuDangKyNghi x)
        {
            return new PhieuDangKyNghiDto(
            x.Id,
            x.NgayNghi,
            x.LyDo,
            x.MaNhanVien,
            x.LoaiChamCongId,
            x.LoaiChamCong?.TenLoai,
            x.LoaiChamCong?.HinhThuc ?? HinhThucNghi.NghiTheoNgay,
            x.UpdatedAt,
            (x as PhieuDangKyNghiTheoGio)?.TuGio,
            (x as PhieuDangKyNghiTheoGio)?.DenGio,
            (x as PhieuDangKyNghiTheoBuoi)?.LoaiBuoi);
        }
    }
}
