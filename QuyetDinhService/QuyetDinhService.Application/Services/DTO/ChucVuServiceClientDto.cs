namespace QuyetDinhService.QuyetDinhService.Application.Services.DTO
{
    public class ChucVuServiceClientDto
    {
        public Guid Id { get; set; }
        public string MaChucVu { get; set; } = string.Empty;
        public string TenChucVu { get; set; } = string.Empty;
        public decimal PhuCap { get; set; }
    }
}
