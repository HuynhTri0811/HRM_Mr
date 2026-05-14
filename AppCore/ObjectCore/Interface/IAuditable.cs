namespace AppCore.ObjectCore.Interface
{
    public interface IAuditable
    {
        DateTime CreatedAt { get; private set; }
        string? CreatedBy { get; private set; }
        DateTime UpdatedAt { get; private set; }
        string? UpdatedBy { get; private set; }
        void Update(string updatedBy);
    }
}