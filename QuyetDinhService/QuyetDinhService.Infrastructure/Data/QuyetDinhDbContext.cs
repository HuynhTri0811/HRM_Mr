using Microsoft.EntityFrameworkCore;
using QuyetDinhService.Domain.Entities;
using QuyetDinhService.Domain.Entities.Interface;
using System.Linq.Expressions;

namespace QuyetDinhService.QuyetDinhService.Infrastructure.Data
{
    public class QuyetDinhDbContext : DbContext
    {
        public QuyetDinhDbContext(DbContextOptions<QuyetDinhDbContext> options) : base(options)
        {
        }


        public DbSet<QuyetDinhNangLuong> QuyetDinhNangLuongs { get; set; }
        public DbSet<QuyetDinhBoNhiem> QuyetDinhBoNhiems { get; set; }

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
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is IAuditable && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                var entity = (IAuditable)entityEntry.Entity;

                if (entityEntry.State == EntityState.Added)
                {
                    entity.Update("123132");
                    // entity.CreatedBy = _currentUserService.UserId; // Lấy từ JWT
                }
                else
                {
                    // Logic cho LastModified
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
