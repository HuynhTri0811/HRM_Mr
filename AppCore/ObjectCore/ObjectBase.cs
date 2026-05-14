using AppCore.ObjectCore.Interface;

namespace AppCore.ObjectCore
{
    public abstract class ObjectBase:IAuditable,IEntity,IDelete
    {
        public Guid Oid { get; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public string CreatedBy { get; private set; } = string.Empty;
        public string UpdatedBy { get; private set; } = string.Empty;
        public bool IsDeleted { get; private set; } = false
        public DateTime? DeletedAt { get; private set; }
        public string? DeletedBy { get; private set; } = string.Empty;
        public ObjectBase()
        {
            Oid = Guid.NewGuid();
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
            CreatedBy = string.Empty;
            UpdatedBy = string.Empty;
        }

        public void Delete(string deletedBy)
        {
            this.IsDeleted = true;
            DeletedAt = DateTime.Now;
            DeletedBy = deletedBy;
        }

        public void Update(string updatedBy)
        {
            UpdatedAt = DateTime.Now;
            UpdatedBy = updatedBy;
        }
    }
}