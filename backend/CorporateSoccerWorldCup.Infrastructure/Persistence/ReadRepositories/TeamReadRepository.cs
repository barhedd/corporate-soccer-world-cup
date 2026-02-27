using CorporateSoccerWorldCup.Application.Common.Pagination;
using CorporateSoccerWorldCup.Application.Features.Teams.Queries.Common.Dtos;
using CorporateSoccerWorldCup.Application.Features.Teams.Queries.Common.Interfaces;
using CorporateSoccerWorldCup.Application.Features.Teams.Queries.GetTeams;
using CorporateSoccerWorldCup.Infrastructure.ConnectionFactories;
using CorporateSoccerWorldCup.Infrastructure.Persistence.Helpers;
using Dapper;

namespace CorporateSoccerWorldCup.Infrastructure.Persistence.ReadRepositories;

public class TeamReadRepository(IDbConnectionFactory dbConnectionFactory) : ITeamReadRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory = dbConnectionFactory;

    public async Task<PagedResult<TeamResponseDto>> GetPagedAsync(
    GetTeamsQuery query,
    CancellationToken cancellationToken)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        var builder = new SqlBuilder();

        var template = builder.AddTemplate(@"
            SELECT /**select**/
            FROM Teams
            /**where**/
            /**orderby**/
            OFFSET @Offset ROWS
            FETCH NEXT @PageSize ROWS ONLY;

            SELECT COUNT(1)
            FROM Teams
            /**where**/
        ");

        builder.Select("Id, Name, ImageUrl");

        builder.Where("IsDeleted = 0");

        if (!string.IsNullOrWhiteSpace(query.Name))
            builder.Where("Name LIKE @Name", new { Name = $"%{query.Name}%" });

        var allowedColumns = new[]
        {
            "Name",
            "CreatedAt"
        };

        var (column, direction) = SqlSortingHelper.BuildSorting(
            query.SortBy,
            query.SortDirection,
            allowedColumns,
            "Name");

        builder.OrderBy($"{column} {direction}");

        var offset = (query.PageNumber - 1) * query.PageSize;

        builder.AddParameters(new
        {
            Offset = offset,
            query.PageSize
        });

        using var multi = await connection.QueryMultipleAsync(
            template.RawSql,
            template.Parameters);

        var data = await multi.ReadAsync<TeamResponseDto>();
        var total = await multi.ReadSingleAsync<int>();

        return new PagedResult<TeamResponseDto>(
            data,
            query.PageNumber,
            query.PageSize,
            total);
    }

    public async Task<TeamResponseDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        const string sql = @"
            SELECT Id, Name, ImageUrl
            FROM Teams
            WHERE Id = @Id
              AND IsDeleted = 0
        ";

        var team = await connection.QuerySingleOrDefaultAsync<TeamResponseDto>(
            sql,
            new { Id = id });

        return team;
    }
}
