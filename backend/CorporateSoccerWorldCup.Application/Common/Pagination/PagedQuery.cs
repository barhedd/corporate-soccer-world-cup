namespace CorporateSoccerWorldCup.Application.Common.Pagination;

public abstract class PagedQuery
{
    private const int MaxPageSize = 50;
    private const int DefaultPageSize = 10;

    private int _pageSize = DefaultPageSize;

    public int PageNumber { get; init; } = 1;

    public int PageSize
    {
        get => _pageSize;
        init => _pageSize = value > MaxPageSize
            ? MaxPageSize
            : value <= 0
                ? DefaultPageSize
                : value;
    }

    public string? SortBy { get; init; }
    public string? SortDirection { get; init; }
}