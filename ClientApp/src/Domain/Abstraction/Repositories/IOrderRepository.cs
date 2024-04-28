using Domain.Entities.OrderDomain;

namespace Domain.Abstraction.Repositories
{
    public interface IOrderRepository
    {
        Task AddOrderAsync(Order order);
    }
}
