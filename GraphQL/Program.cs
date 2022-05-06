using ConferencePlanner.GraphQL;
using ConferencePlanner.GraphQL.Data;
using ConferencePlanner.GraphQL.DataLoader;
using ConferencePlanner.GraphQL.Types;
using Microsoft.EntityFrameworkCore;
using ConferencePlanner.GraphQL.Speakers;
using ConferencePlanner.GraphQL.Sessions;
using ConferencePlanner.GraphQL.Tracks;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPooledDbContextFactory<ApplicationDbContext>(options => options.UseSqlite("Data Source=conferences.db"));

builder.Services
    .AddGraphQLServer()
    .EnableRelaySupport()
    .AddQueryType(d => d.Name("Query"))
        .AddTypeExtension<SessionQueries>()
        .AddTypeExtension<SpeakerQueries>()
        .AddTypeExtension<TrackQueries>()
     .AddMutationType(d => d.Name("Mutation"))
        .AddTypeExtension<SessionMutations>()
        .AddTypeExtension<SpeakerMutations>()
        .AddTypeExtension<TrackMutations>()
     .AddType<AttendeeType>()
     .AddType<SessionType>()
     .AddType<SpeakerType>()
     .AddType<TrackType>();

var app = builder.Build();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGraphQL();
});

app.Run();
