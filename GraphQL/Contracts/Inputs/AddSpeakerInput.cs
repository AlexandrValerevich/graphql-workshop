namespace ConferencePlanner.GraphQL.Contracts.Inputs;

public record AddSpeakerInput(
    string Name,
    string Bio,
    string WebSite);