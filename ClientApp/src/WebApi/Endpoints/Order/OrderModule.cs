using Application.Orders.Commands;
using Carter;
using MediatR;

namespace WebApi.Endpoints.Order
{
    public class OrderModule : CarterModule
    {
        public OrderModule() : base("/order")
        {

        }

        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/", async (CreateOrderCommand command, IMediator sender) =>
            {
                await sender.Send(command);
                return Results.Ok();
            }).RequireAuthorization();
        }
    }
}
