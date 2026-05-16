using ChamCongService.Domain.Entity;

namespace ChamCongService.Application.DTOs
{
    public record LoaiChamCongDto(Guid Id, string MaLoai, string TenLoai, double HeSo, HinhThucNghi HinhThuc);
}
