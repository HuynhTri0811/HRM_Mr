using Microsoft.EntityFrameworkCore;
using TinhLuongService.Models;

namespace TinhLuongService.Data
{
    public class TinhLuongDbContext : DbContext
    {
        public TinhLuongDbContext(DbContextOptions<TinhLuongDbContext> options) : base(options) { }

        public DbSet<SalaryRecord> SalaryRecords { get; set; }
    }
}
