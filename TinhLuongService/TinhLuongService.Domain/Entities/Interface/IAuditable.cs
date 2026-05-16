namespace TinhLuongService.Domain.Entities.Interface
{
    public interface IAuditable
    {
        DateTime CreatedAt { get; }
        string? CreatedBy { get; }
        DateTime UpdatedAt { get; }
        string? UpdatedBy { get; }
        void Update(string updatedBy);
    }
}