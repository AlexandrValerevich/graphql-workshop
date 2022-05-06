using Microsoft.EntityFrameworkCore;
using ConferencePlanner.GraphQL.Data;
using ConferencePlanner.GraphQL.DataLoader;

namespace ConferencePlanner.GraphQL.Sessions;

[ExtendObjectType("Query")]
public class SessionQueries
{
    [UseApplicationDbContext]
    public async Task<IEnumerable<Session>> GetSessionsAsync(
        [ScopedService] ApplicationDbContext context,
        CancellationToken cancellationToken)
    {
        return await context.Sessions.ToListAsync(cancellationToken);
    }

    public Task<Session> GetSessionByIdAsync(
        [ID(nameof(Session))] int id,
        SessionByIdDataLoader sessionById,
        CancellationToken cancellationToken)
    {
        return sessionById.LoadAsync(id, cancellationToken);
    }

    public async Task<IEnumerable<Session>> GetSessionsByIdAsync(
        [ID(nameof(Session))] int[] ids,
        SessionByIdDataLoader sessionById,
        CancellationToken cancellationToken)
    {
        return await sessionById.LoadAsync(ids, cancellationToken);
    }
}