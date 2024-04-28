namespace Domain.Exceptions.Base
{
    // Custom exception allow us to build more descriptive error message
    // and perform logging to debug the error more easily
    public class DomainException : Exception
    {
        public DomainException(string message) : base(message)
        {
        }
    }
}
