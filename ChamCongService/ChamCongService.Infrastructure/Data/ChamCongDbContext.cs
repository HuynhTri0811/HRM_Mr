using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ChamCongService.Domain.Entity;
using ChamCongService.Domain.Entity.Interface;
using ChamCongService.Domain.Entity.Base;
using ChamCongService.Common;

namespace ChamCongService.ChamCongService.Infrastructure.Data
{
    public class ChamCongDbContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ChamCongDbContext(DbContextOptions<ChamCongDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public DbSet<AttendanceRecord> AttendanceRecords { get; set; }
        public DbSet<LoaiChamCong> LoaiChamCongs { get; set; }
        public DbSet<PhieuDangKyNghi> PhieuDangKyNghis { get; set; }
        public DbSet<PhieuDangKyNghiTheoGio> PhieuDangKyNghiTheoGios { get; set; }
        public DbSet<PhieuDangKyNghiTheoBuoi> PhieuDangKyNghiTheoBuois { get; set; }
        public DbSet<PhieuDangKyNghiTheoNgay> PhieuDangKyNghiTheoNgays { get; set; }
        public DbSet<BangChamCongTheoThang> BangChamCongTheoThangs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(new SoftInterceptor(_httpContextAccessor));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Tự động lọc các bản ghi đã xóa cho toàn bộ các bảng có dùng IDelete
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(IDelete).IsAssignableFrom(entityType.ClrType) && entityType.BaseType == null)
                {
                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(ConvertFilterExpression(entityType.ClrType));
                }

                // Cấu hình concurrency token dựa trên UpdatedAt
                if (typeof(ObjectBase).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.ClrType)
                        .Property(nameof(ObjectBase.UpdatedAt))
                        .IsConcurrencyToken();
                }
            }
        }

        private LambdaExpression ConvertFilterExpression(Type entityType)
        {
            // Tạo biểu thức: e => !e.IsDeleted
            var parameter = Expression.Parameter(entityType, "e");
            var property = Expression.Property(parameter, nameof(IDelete.IsDeleted));
            var body = Expression.Not(property);
            return Expression.Lambda(body, parameter);
        }
    }
}
