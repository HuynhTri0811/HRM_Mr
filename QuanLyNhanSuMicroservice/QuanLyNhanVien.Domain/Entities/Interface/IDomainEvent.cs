namespace QuanLyNhanSuMicroservice.QuanLyNhanVien.Domain.Entities.Interface
{
    public interface IDomainEvent
    {
        DateTime OccurredOn { get; }
    }
}
