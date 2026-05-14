using Microsoft.EntityFrameworkCore;
using ChamCongService.Models;

namespace ChamCongService.Data
{
    public class ChamCongDbContext : DbContext
    {
        public ChamCongDbContext(DbContextOptions<ChamCongDbContext> options) : base(options) { }

        public DbSet<AttendanceRecord> AttendanceRecords { get; set; }
    }
}
