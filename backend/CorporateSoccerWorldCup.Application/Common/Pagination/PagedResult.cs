namespace CorporateSoccerWorldCup.Application.Common.Pagination;

public sealed class PagedResult<T>(
    IEnumerable<T> data,
    int pageNumber,
    int pageSize,
    int totalRecords)
{
    public IEnumerable<T> Data { get; } = data;
    public int PageNumber { get; } = pageNumber;
    public int PageSize { get; } = pageSize;
    public int TotalRecords { get; } = totalRecords;
    public int TotalPages { get; } = (int)Math.Ceiling(totalRecords / (double)pageSize);
}