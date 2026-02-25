namespace CorporateSoccerWorldCup.Infrastructure.Persistence.Helpers;

public static class SqlSortingHelper
{
    public static (string Column, string Direction) BuildSorting(
        string? sortBy,
        string? sortDirection,
        IEnumerable<string> allowedColumns,
        string defaultColumn)
    {
        var column = allowedColumns.Contains(sortBy)
            ? sortBy!
            : defaultColumn;

        var direction = sortDirection?.ToLower() == "desc"
            ? "DESC"
            : "ASC";

        return (column, direction);
    }
}
