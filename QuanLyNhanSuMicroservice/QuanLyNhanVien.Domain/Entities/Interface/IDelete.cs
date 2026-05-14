namespace QuanLyNhanSuMicroservice.QuanLyNhanVien.Domain.Entities.Interface
{
    public interface IDelete
    {
        bool IsDeleted { get; }
        DateTime? DeletedAt { get; }
        string? DeletedBy { get; }

        void Delete(string deletedBy);
    }
}