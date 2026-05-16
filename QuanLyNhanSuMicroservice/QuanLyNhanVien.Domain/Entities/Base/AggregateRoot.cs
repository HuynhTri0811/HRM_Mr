using QuanLyNhanSuMicroservice.QuanLyNhanVien.Domain.Entities.Interface;

namespace QuanLyNhanSuMicroservice.QuanLyNhanVien.Domain.Entities.Base
{
    public abstract class AggregateRoot : IHasDomainEvents
    {
        private readonly List<IDomainEvent> _domainEvents = new();
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        protected void RaiseDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
}
