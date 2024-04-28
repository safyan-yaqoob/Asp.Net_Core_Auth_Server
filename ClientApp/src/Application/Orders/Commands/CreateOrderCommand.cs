using MediatR;

namespace Application.Orders.Commands
{
    public sealed record CreateOrderCommand(IList<CreateOrderItemCommand> OrderItems) : IRequest<Unit>;
}
