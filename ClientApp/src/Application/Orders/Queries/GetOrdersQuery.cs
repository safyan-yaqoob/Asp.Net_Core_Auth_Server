using MediatR;

namespace Application.Orders.Queries
{
    public sealed record GetOrdersQuery: IRequest<GetOrdersQueryResponse>
    {
    }

    public sealed record GetOrdersQueryResponse 
    {

    }
}
