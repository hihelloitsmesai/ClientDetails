using ClientDirectory.Application.Client.Commands.CreateClient;
using ClientDirectory.Application.Client.Models;
using ClientDirectory.Application.Client.Queries.GetClients;
using ClientDirectory.Application.TodoItems.Queries.GetTodoItemsWithPagination;

namespace ClientDirectory.Web.Endpoints;

public class ClientDetails : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization()
            .MapGet(GetClientLists)
            .MapPost(CreateClient);
    }

    public Task<List<ClientVm>> GetClientLists(ISender sender, [AsParameters] GetClientsQuery search)
    {
        return sender.Send(search);
    }

    public Task<int> CreateClient(ISender sender, CreateClientCommand command)
    {
        return sender.Send(command);
    }

}
