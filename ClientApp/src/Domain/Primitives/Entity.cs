namespace Domain.Primitives
{
    public abstract class Entity
    {
        public Entity(Guid id)
        {
            Id = id;
        }

        // used init here so the Id of the entity can only once when object initialize
        // and cannot be modified thereafter
        // and also made it private so that it can only be set from inside the entity class
        public Guid Id { get; private init; }
    }
}