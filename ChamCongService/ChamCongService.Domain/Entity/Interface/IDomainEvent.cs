namespace ChamCongService.Domain.Entity.Interface
{
    public interface IDomainEvent
    {
        DateTime OccurredOn { get; }
    }
}
