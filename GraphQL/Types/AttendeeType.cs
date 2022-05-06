using Microsoft.EntityFrameworkCore;
using ConferencePlanner.GraphQL.Data;
using ConferencePlanner.GraphQL.DataLoader;

namespace ConferencePlanner.GraphQL.Types;

#pragma warning disable 

public class AttendeeType : ObjectType<Attendee>
{
    protected override void Configure(IObjectTypeDescriptor<Attendee> descriptor)
    {
        descriptor
            .ImplementsNode()
            .IdField(t => t.Id)
            .ResolveNode(async (ctx, id) => await ctx.DataLoader<AttendeeByIdDataLoader>().LoadAsync(id, ctx.RequestAborted));

        descriptor
            .Field(t => t.Sessions)
            .ResolveWith<AttendeeResolvers>(t => t.GetSessionsAsync(default!, default!, default!, default))
            .UseDbContext<ApplicationDbContext>()
            .Name("sessions");
    }

    // It is has no sense because configuration of 
    // our db models supoort it out of the box 
    private class AttendeeResolvers
    {
        public async Task<IEnumerable<Session>> GetSessionsAsync(
            Speaker speaker,
            [ScopedService] ApplicationDbContext dbContext,
            SessionByIdDataLoader sessionById,
            CancellationToken cancellationToken)
        {
            int[] sessionIds = await dbContext.Speakers.Where(s => s.Id.Equals(speaker.Id))
                                                       .Include(sp => sp.Sessions)
                                                       .SelectMany(sp => sp.Sessions.Select(ss => ss.Id))
                                                       .ToArrayAsync(cancellationToken: cancellationToken);

            return await sessionById.LoadAsync(sessionIds, cancellationToken);
        }
    }
}