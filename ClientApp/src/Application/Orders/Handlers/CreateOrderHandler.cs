using Application.Orders.Commands;
using Domain.Abstraction;
using Domain.Abstraction.Repositories;
using Domain.Entities.OrderDomain;
using FluentValidation;
using MediatR;

namespace Application.Orders.Handlers
{

    internal class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(x => x.OrderItems)
                .NotEmpty()
                .WithMessage("Order should contain atleast one item.");
        }
    }

    public sealed class CreateOrderHandler : IRequestHandler<CreateOrderCommand, Unit>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;
        public CreateOrderHandler(IOrderRepository orderRepository,
            IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var items = request.OrderItems.Select(e => OrderItem.Create(e.productName)).ToList();
            
            var order = Order.Create(items);

            await _orderRepository.AddOrderAsync(order);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
