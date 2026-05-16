namespace QuyetDinhService.Domain.Entities.Interface
{
    public interface IDomainEvent
    {
        DateTime OccurredOn { get; }
    }
}
