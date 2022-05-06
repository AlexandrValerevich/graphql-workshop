using ConferencePlanner.GraphQL.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GraphQL.Data.Configure;

public class SessionConfigure : IEntityTypeConfiguration<Session>
{
    public void Configure(EntityTypeBuilder<Session> builder)
    {
        builder.HasMany(ss => ss.Attendees)
               .WithMany(at => at.Sessions);

        builder.HasMany(ss => ss.Speakers)
                .WithMany(sp => sp.Sessions);

        builder.HasOne(ss => ss.Track)
               .WithMany(t => t.Sessions);

        
    }
}