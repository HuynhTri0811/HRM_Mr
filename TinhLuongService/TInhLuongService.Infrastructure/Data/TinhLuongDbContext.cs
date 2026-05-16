using Microsoft.EntityFrameworkCore;
using TinhLuongService.Domain.Entities;
using System.Linq.Expressions;
using System.Security.Claims;
using TinhLuongService.Domain.Entities.Interface;
using Microsoft.AspNetCore.Http;

namespace TinhLuongService.Data
{
    public class TinhLuongDbContext : DbContext
    {

        public DbSet<KyTinhLuong> KyTinhLuongs { get; set; }
        public DbSet<NhanVienTinhLuong> NhanVienTinhLuongs { get; set; }

        private readonly IHttpContextAccessor _httpContextAccessor;

        public TinhLuongDbContext(DbContextOptions<TinhLuongDbContext> options, IHttpContextAccessor httpContextAccessor)
        : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(new SoftInterceptor(_httpContextAccessor));
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Tự động lọc các bản ghi đã xóa cho toàn bộ các bảng có dùng ISoftDelete
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(IDelete).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(ConvertFilterExpression(entityType.ClrType));
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


        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var user = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value
                      ?? _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                      ?? "System";

            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is IAuditable && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                var entity = (IAuditable)entityEntry.Entity;

                if (entityEntry.State == EntityState.Added)
                {
                    entity.Update(user);
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
