using ClientDirectory.Application.Common.Interfaces;
using ClientDirectory.Domain.Entities;

namespace ClientDirectory.Application.Client.Commands.CreateClient;

public record CreateClientCommand : IRequest<int>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string MobileNumber { get; set; } = string.Empty;
    public string IdNumber { get; set; } = string.Empty;
    public string PhysicalAddress { get; set; } = string.Empty;
}



public class CreateClientCommandHandler : IRequestHandler<CreateClientCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateClientCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateClientCommand request, CancellationToken cancellationToken)
    {
        var entity = new Domain.Entities.Client();

        entity.FirstName = request.FirstName;
        entity.LastName = request.LastName;
        entity.MobileNumber = request.MobileNumber;
        entity.IdNumber = request.IdNumber;
        entity.PhysicalAddress = request.PhysicalAddress;

        _context.Clients.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
