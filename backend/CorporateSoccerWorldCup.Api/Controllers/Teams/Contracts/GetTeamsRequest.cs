namespace CorporateSoccerWorldCup.Api.Controllers.Teams.Contracts;

public record GetTeamsRequest
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public string? SortBy { get; init; }
    public string? SortDirection { get; init; }

    // filtros
    public string? Name { get; init; }
}
