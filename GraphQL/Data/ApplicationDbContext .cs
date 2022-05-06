using GraphQL.Data.Configure;
using Microsoft.EntityFrameworkCore;

namespace ConferencePlanner.GraphQL.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Session> Sessions { get; set; } = default!;
    
    public DbSet<Track> Tracks { get; set; } = default!;
    
    public DbSet<Speaker> Speakers { get; set; } = default!;
    
    public DbSet<Attendee> Attendees { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new AttendeeConfigure());
        builder.ApplyConfiguration(new SessionConfigure());
        builder.ApplyConfiguration(new SpeakerConfigure());
        builder.ApplyConfiguration(new TrackConfigure());

    }
}