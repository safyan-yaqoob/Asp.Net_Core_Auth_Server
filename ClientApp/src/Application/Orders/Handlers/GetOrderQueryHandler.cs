using Application.Orders.Queries;
using MediatR;

namespace Application.Orders.Handlers
{
    public sealed class GetOrderQueryHandler : IRequestHandler<GetOrdersQuery, GetOrdersQueryResponse>
    {
        public Task<GetOrdersQueryResponse> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
