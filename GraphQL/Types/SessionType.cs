using Microsoft.EntityFrameworkCore;
using ConferencePlanner.GraphQL.Data;
using ConferencePlanner.GraphQL.DataLoader;

#pragma warning disable 

namespace ConferencePlanner.GraphQL.Types;

public class SessionType : ObjectType<Session>
{
    protected override void Configure(IObjectTypeDescriptor<Session> descriptor)
    {
        descriptor
            .ImplementsNode()
            .IdField(t => t.Id)
            .ResolveNode(async (ctx, id) => await ctx.DataLoader<SessionByIdDataLoader>().LoadAsync(id, ctx.RequestAborted));

        descriptor
            .Field(t => t.Speakers)
            .ResolveWith<SessionResolvers>(t => t.GetSpeakersAsync(default!, default!, default!, default))
            .UseDbContext<ApplicationDbContext>()
            .Name("speakers");

        descriptor
            .Field(t => t.Attendees)
            .ResolveWith<SessionResolvers>(t => t.GetAttendeesAsync(default!, default!, default!, default))
            .UseDbContext<ApplicationDbContext>()
            .Name("attendees");

        descriptor
            .Field(t => t.Track)
            .ResolveWith<SessionResolvers>(t => t.GetTrackAsync(default!, default!, default));
    }


     // It is has no sense because configuration of 
    // our db models supoort it out of the box 
    private class SessionResolvers
    {
        public async Task<IEnumerable<Speaker>> GetSpeakersAsync(
            Session session,
            [ScopedService] ApplicationDbContext dbContext,
            SpeakerByIdDataLoader speakerById,
            CancellationToken cancellationToken)
        {
            int[] speakerIds = await dbContext.Sessions
                .Where(s => s.Id.Equals(session.Id))
                .Include(s => s.Speakers)
                .SelectMany(s => s.Speakers.Select(sp => sp.Id))
                .ToArrayAsync(cancellationToken: cancellationToken);

            return await speakerById.LoadAsync(speakerIds, cancellationToken);
        }

        public async Task<IEnumerable<Attendee>> GetAttendeesAsync(
            Session session,
            [ScopedService] ApplicationDbContext dbContext,
            AttendeeByIdDataLoader attendeeById,
            CancellationToken cancellationToken)
        {
            int[] attendeeIds = await dbContext.Sessions
                .Where(s => s.Id == session.Id)
                .Include(session => session.Attendees)
                .SelectMany(session => session.Attendees.Select(a => a.Id))
                .ToArrayAsync(cancellationToken: cancellationToken);

            return await attendeeById.LoadAsync(attendeeIds, cancellationToken);
        }

        // it really stypid pice of code
        public async Task<Track?> GetTrackAsync(
            Session session,
            TrackByIdDataLoader trackById,
            CancellationToken cancellationToken)
        {
            if (session.Track is null)
            {
                return null;
            }

            return await trackById.LoadAsync(session.Track.Id, cancellationToken);
        }
    }
}