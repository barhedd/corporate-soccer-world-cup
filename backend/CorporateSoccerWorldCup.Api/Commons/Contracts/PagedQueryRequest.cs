namespace CorporateSoccerWorldCup.Api.Commons.Contracts;

public abstract class PagedQueryRequest
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public string? SortBy { get; init; }
    public string? SortDirection { get; init; }
}
