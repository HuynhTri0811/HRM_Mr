
using TinhLuongService.Domain.Entities.Interface;

namespace TinhLuongService.Domain.Entities
{
    public abstract class ObjectBase : IAuditable, IEntity, IDelete
    {
        public Guid Id { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public string CreatedBy { get; private set; } = string.Empty;
        public string UpdatedBy { get; private set; } = string.Empty;
        public bool IsDeleted { get; private set; } = false;
        public DateTime? DeletedAt { get; private set; }
        public string? DeletedBy { get; private set; } = string.Empty;
        public ObjectBase()
        {
            Id = Guid.NewGuid();
            var now = DateTime.UtcNow;
            var nowCoarse = new DateTime(now.Ticks - (now.Ticks % TimeSpan.TicksPerMillisecond), DateTimeKind.Utc);
            CreatedAt = nowCoarse;
            UpdatedAt = nowCoarse;
            CreatedBy = string.Empty;
            UpdatedBy = string.Empty;
        }

        public void Delete(string deletedBy)
        {
            this.IsDeleted = true;
            var now = DateTime.UtcNow;
            DeletedAt = new DateTime(now.Ticks - (now.Ticks % TimeSpan.TicksPerMillisecond), DateTimeKind.Utc);
            DeletedBy = deletedBy;
        }

        public void Update(string updatedBy)
        {
            var now = DateTime.UtcNow;
            UpdatedAt = new DateTime(now.Ticks - (now.Ticks % TimeSpan.TicksPerMillisecond), DateTimeKind.Utc);
            UpdatedBy = updatedBy;
        }
    }
}