using Microsoft.EntityFrameworkCore;
using ChamCongService.Domain.Entity;

namespace ChamCongService.ChamCongService.Infrastructure.Data
{
    public class ChamCongDbContext : DbContext
    {
        public ChamCongDbContext(DbContextOptions<ChamCongDbContext> options) : base(options) { }

        public DbSet<AttendanceRecord> AttendanceRecords { get; set; }
        public DbSet<LoaiChamCong> LoaiChamCongs { get; set; }
        public DbSet<PhieuDangKyNghi> PhieuDangKyNghis { get; set; }
        public DbSet<PhieuDangKyNghiTheoGio> PhieuDangKyNghiTheoGios { get; set; }
        public DbSet<PhieuDangKyNghiTheoBuoi> PhieuDangKyNghiTheoBuois { get; set; }
        public DbSet<PhieuDangKyNghiTheoNgay> PhieuDangKyNghiTheoNgays { get; set; }
        public DbSet<BangChamCongTheoThang> BangChamCongTheoThangs { get; set; }
    }
}
