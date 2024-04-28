using Domain.Primitives;

namespace Domain.Entities.OrderDomain
{
    // Aggregate is group of related entites, it's a domain driven design concept
    // which represent the transactional boundary of our (order example in this case)
    // to ensure that user cannot fetach directly child objects from db.
    public sealed class Order : AggregateRoot
    {
        private List<OrderItem> _items = new ();
        public Order(Guid id) : base(id)
        {
        }

        public IReadOnlyList<OrderItem> Items => _items;
        public static Order Create(IList<OrderItem>? items = null)
        {
            var order = new Order(Guid.NewGuid());
            order.AddItems(items);
            return order;
        }

        public void RemoveItem(OrderItem item) => _items.Remove(item);
        public void AddItems(IList<OrderItem>? items) 
        {
            if(items == null)
            {
                _items = null!;
                return;
            }
            _items.AddRange(items);
        }
    }
}