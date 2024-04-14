using ClientDirectory.Application.Client.Models;
using ClientDirectory.Application.Common.Interfaces;

namespace ClientDirectory.Application.Client.Queries.GetClients;

public record GetClientsQuery : IRequest<List<ClientVm>>
{
    public string Search { get; set; } = string.Empty;
}

public class GetClientsQueryValidator : AbstractValidator<GetClientsQuery>
{
    public GetClientsQueryValidator()
    {
    }
}

public class GetClientsQueryHandler : IRequestHandler<GetClientsQuery, List<ClientVm>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetClientsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ClientVm>> Handle(GetClientsQuery request, CancellationToken cancellationToken)
    {

        var result = new List<ClientVm>();

        if (string.IsNullOrEmpty(request.Search))
        {

            result = await _context.Clients
                    .AsNoTracking()
                    .ProjectTo<ClientVm>(_mapper.ConfigurationProvider)
                    .OrderBy(t => t.FirstName)
                    .ToListAsync(cancellationToken);
        }
        else
        {
            var searchKey = request.Search.ToLower();
            result = await _context.Clients
                        .AsNoTracking()
                        .ProjectTo<ClientVm>(_mapper.ConfigurationProvider)
                        .Where(x => x.FirstName.ToLower().Contains(searchKey) || x.IdNumber.ToLower().Contains(searchKey) || x.MobileNumber.ToLower().Contains(searchKey))
                        .ToListAsync(cancellationToken);
        }

        return result;
    }
}
