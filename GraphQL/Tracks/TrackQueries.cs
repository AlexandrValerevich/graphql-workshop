using Microsoft.EntityFrameworkCore;
using ConferencePlanner.GraphQL.Data;
using ConferencePlanner.GraphQL.DataLoader;

namespace ConferencePlanner.GraphQL.Tracks;

[ExtendObjectType("Query")]
public class TrackQueries
{
    [UseApplicationDbContext]
    [UsePaging]
    public IQueryable<Track> GetTracks([ScopedService] ApplicationDbContext context)
    {
        return context.Tracks.OrderBy(t => t.Name);
    }

    [UseApplicationDbContext]
    public Task<Track> GetTrackByNameAsync(string name,
                                           [ScopedService] ApplicationDbContext context,
                                           CancellationToken cancellationToken)
    {
        return context.Tracks.FirstAsync(t => t.Name == name, cancellationToken);
    }

    [UseApplicationDbContext]
    public async Task<IEnumerable<Track>> GetTrackByNamesAsync(string[] names,
                                                               [ScopedService] ApplicationDbContext context,
                                                               CancellationToken cancellationToken)
    {
        return await context.Tracks.Where(t => names.Contains(t.Name)).ToListAsync(cancellationToken);
    }

    public Task<Track> GetTrackByIdAsync([ID(nameof(Track))] int id,
                                         TrackByIdDataLoader trackById,
                                         CancellationToken cancellationToken)
    {
        return trackById.LoadAsync(id, cancellationToken);
    }

    public async Task<IEnumerable<Track>> GetTracksByIdAsync([ID(nameof(Track))] int[] ids,
                                                             TrackByIdDataLoader trackById,
                                                             CancellationToken cancellationToken)
    {
        return await trackById.LoadAsync(ids, cancellationToken);
    }
}