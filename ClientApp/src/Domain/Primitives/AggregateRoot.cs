
namespace Domain.Primitives
{
    public abstract class AggregateRoot : Entity
    {
        private readonly List<IDomainEvent> _domainEvents = new ();
        protected AggregateRoot(Guid id) : base(id)
        {
        }

        public void RaiseDomainEvent(IDomainEvent Event)
        {
            _domainEvents.Add(Event);
        }

        public List<IDomainEvent> GetDomainEvents()
        {
            return _domainEvents;
        }
    }
}
