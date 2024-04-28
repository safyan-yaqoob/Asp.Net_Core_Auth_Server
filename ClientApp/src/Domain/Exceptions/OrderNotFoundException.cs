using Domain.Exceptions.Base;

namespace Domain.Exceptions
{
    public class OrderNotFoundException : DomainException
    {
        public OrderNotFoundException(string message) : base(message)
        {
        }
    }
}
