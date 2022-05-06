using Microsoft.EntityFrameworkCore;
using ConferencePlanner.GraphQL.Data;
using ConferencePlanner.GraphQL.DataLoader;

#pragma warning disable 

namespace ConferencePlanner.GraphQL.Types;

public class TrackType : ObjectType<Track>
{
    protected override void Configure(IObjectTypeDescriptor<Track> descriptor)
    {
        descriptor
            .ImplementsNode()
            .IdField(t => t.Id)
            .ResolveNode(async (ctx, id) =>
                await ctx.DataLoader<TrackByIdDataLoader>().LoadAsync(id, ctx.RequestAborted));

        descriptor
            .Field(t => t.Name)
            .UseUpperCase();

            

        descriptor
            .Field(t => t.Sessions)
            .ResolveWith<TrackResolvers>(t => t.GetSessionsAsync(default!, default!, default!, default))
            .UseDbContext<ApplicationDbContext>()
            .UsePaging<NonNullType<SessionType>>()
            .Name("sessions");
    }

    private class TrackResolvers
    {
        public async Task<IEnumerable<Session>> GetSessionsAsync(
            Track track,
            [ScopedService] ApplicationDbContext dbContext,
            SessionByIdDataLoader sessionById,
            CancellationToken cancellationToken)
        {
            int[] sessionIds = await dbContext.Tracks
                .Where(t => t.Id == track.Id)
                .SelectMany(t => t.Sessions.Select(ss => ss.Id))
                .ToArrayAsync(cancellationToken: cancellationToken);

            return await sessionById.LoadAsync(sessionIds, cancellationToken);
        }
    }
}