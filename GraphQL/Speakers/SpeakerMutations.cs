using ConferencePlanner.GraphQL.Data;

namespace ConferencePlanner.GraphQL.Speakers;


[ExtendObjectType("Mutation")]
public class SpeakerMutations
{
    [UseApplicationDbContext]
    public async Task<SpeakerPayloadBase> AddSpeakerAsync(
        AddSpeakerInput input,
        [ScopedService] ApplicationDbContext context)
    {
        var speaker = new Speaker
        {
            Name = input.Name,
            Bio = input.Bio,
            WebSite = input.WebSite
        };

        context.Speakers.Add(speaker);
        await context.SaveChangesAsync();

        return new AddSpeakerPayload(speaker);
    }
}