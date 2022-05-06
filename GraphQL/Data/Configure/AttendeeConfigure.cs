using ConferencePlanner.GraphQL.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GraphQL.Data.Configure;

public class AttendeeConfigure : IEntityTypeConfiguration<Attendee>
{
    public void Configure(EntityTypeBuilder<Attendee> builder)
    {
        builder.HasIndex(a => a.UserName)
               .IsUnique();

        builder.HasMany(a => a.Sessions)
               .WithMany(s => s.Attendees);
    }
}