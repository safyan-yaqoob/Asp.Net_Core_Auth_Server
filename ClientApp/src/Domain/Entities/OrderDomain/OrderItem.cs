using Domain.Primitives;

namespace Domain.Entities.OrderDomain
{
    public class OrderItem : Entity
    {
        public OrderItem(Guid id) : base(id)
        {
        }
        public string ProductName { get; private set; } = default!;
        public Order Order { get; private set; } = null!;
        public Guid OrderId { get; private set; }

        public static OrderItem Create(string productName)
        {
            var orderItem = new OrderItem(Guid.NewGuid());
            orderItem.ProductName = productName;
            return orderItem;
        }
    }
}
