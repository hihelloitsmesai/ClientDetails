using ClientDirectory.Domain.Entities;

namespace ClientDirectory.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<TodoList> TodoLists { get; }

    DbSet<TodoItem> TodoItems { get; }

    DbSet<ClientDirectory.Domain.Entities.Client> Clients { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
