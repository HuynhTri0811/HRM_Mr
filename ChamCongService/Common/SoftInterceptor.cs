using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ChamCongService.Domain.Entity.Interface;

namespace ChamCongService.Common
{
    public class SoftInterceptor : SaveChangesInterceptor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SoftInterceptor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public override InterceptionResult<int> SavingChanges(
            DbContextEventData eventData,
            InterceptionResult<int> result)
        {
            ApplySoftDelete(eventData);
            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            ApplySoftDelete(eventData);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private void ApplySoftDelete(DbContextEventData eventData)
        {
            if (eventData.Context is null) return;

            var user = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value 
                      ?? _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                      ?? "System";

            foreach (var entry in eventData.Context.ChangeTracker.Entries<IDelete>())
            {
                if (entry.State == EntityState.Deleted)
                {
                    entry.State = EntityState.Modified;
                    entry.Entity.Delete(user);
                }
            }
            foreach (var entry in eventData.Context.ChangeTracker.Entries<IAuditable>())
            {
                if (entry.State == EntityState.Modified)
                {
                    entry.State = EntityState.Modified;
                    entry.Entity.UpdateBase(user);
                }
            }
        }
    }
}
