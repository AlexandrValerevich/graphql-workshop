using ConferencePlanner.GraphQL.Data;
using ConferencePlanner.GraphQL.DataLoader;
using Microsoft.EntityFrameworkCore;


namespace ConferencePlanner.GraphQL.Speakers;

[ExtendObjectType("Query")]
public class SpeakerQueries
{
    [UseApplicationDbContext]
    public Task<List<Speaker>> GetSpeakers([ScopedService] ApplicationDbContext context)
    {
        return context.Speakers.ToListAsync();
    }

    public Task<Speaker> GetSpeakerByIdAsync(
        [ID(nameof(Speaker))] int id,
        SpeakerByIdDataLoader dataLoader,
        CancellationToken cancellationToken)
    {
        return dataLoader.LoadAsync(id, cancellationToken);
    }

    public async Task<IEnumerable<Speaker>> GetSpeakersByIdAsync(
            [ID(nameof(Speaker))] int[] ids,
            SpeakerByIdDataLoader dataLoader,
            CancellationToken cancellationToken)
    {
        return await dataLoader.LoadAsync(ids, cancellationToken);
    }
}