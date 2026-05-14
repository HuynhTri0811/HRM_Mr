
using QuanLyNhanSuMicroservice.QuanLyNhanVien.Domain.Entities.Interface;

namespace QuanLyNhanSuMicroservice
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
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            CreatedBy = string.Empty;
            UpdatedBy = string.Empty;
        }

        public void Delete(string deletedBy)
        {
            this.IsDeleted = true;
            DeletedAt = DateTime.UtcNow;
            DeletedBy = deletedBy;
        }

        public void UpdateBase(string updatedBy)
        {
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = updatedBy;
        }
    }
}