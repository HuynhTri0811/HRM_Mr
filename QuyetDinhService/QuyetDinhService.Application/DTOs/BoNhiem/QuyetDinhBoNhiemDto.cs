using QuyetDinhService.QuyetDinhService.Application.Services.DTO;

namespace QuyetDinhService.QuyetDinhService.Application.DTOs.BoNhiem
{
    public record QuyetDinhBoNhiemDto(
        Guid Id, 
        string SoQuyetDinh, 
        DateTime NgayHieuLuc, 
        string NoiDung, 
        string GhiChu, 
        NhanVienServiceClientDto NhanVien, 
        Guid ChucVuCu, 
        Guid ChucVuMoi, 
        decimal PhuCapCu, 
        decimal PhuCapMoi, 
        string LyDo,
        DateTime UpdatedAt);
}
