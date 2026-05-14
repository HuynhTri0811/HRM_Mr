namespace AppCore.ObjectCore.Interface
{
    public interface IDelete
    {
        bool IsDeleted { get; set; }
        DateTime? DeletedAt { get; set; }
        string? DeletedBy { get; set; }
    }
}