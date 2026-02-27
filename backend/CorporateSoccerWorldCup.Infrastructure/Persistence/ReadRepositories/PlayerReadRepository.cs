using CorporateSoccerWorldCup.Application.Common.Pagination;
using CorporateSoccerWorldCup.Application.Features.Players.Queries.Common.Dtos;
using CorporateSoccerWorldCup.Application.Features.Players.Queries.Common.Interfaces;
using CorporateSoccerWorldCup.Application.Features.Players.Queries.GetPlayers;
using CorporateSoccerWorldCup.Infrastructure.ConnectionFactories;
using CorporateSoccerWorldCup.Infrastructure.Persistence.Helpers;
using Dapper;

namespace CorporateSoccerWorldCup.Infrastructure.Persistence.ReadRepositories;

public class PlayerReadRepository(IDbConnectionFactory dbConnectionFactory) : IPlayerReadRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory = dbConnectionFactory;

    public async Task<PagedResult<PlayerResponseDto>> GetPagedAsync(GetPlayersQuery query, CancellationToken cancellationToken)
    {
        using var connection = _dbConnectionFactory.CreateConnection();
        var builder = new SqlBuilder();

        var template = builder.AddTemplate(@"
            SELECT /**select**/
            FROM Players p
            INNER JOIN Teams t ON p.TeamId = t.Id
            INNER JOIN PlayerStatuses s ON p.StatusId = s.Id
            /**where**/
            /**orderby**/
            OFFSET @Offset ROWS
            FETCH NEXT @PageSize ROWS ONLY;

            SELECT COUNT(1)
            FROM Players p
            INNER JOIN Teams t ON p.TeamId = t.Id
            INNER JOIN PlayerStatuses s ON p.StatusId = s.Id
            /**where**/
        ");

        builder.Select(@"
            p.Id,
            p.Name,
            p.Birthday,
            p.TeamId,
            t.Name AS TeamName,
            p.StatusId,
            s.Name AS StatusName,
            p.SanctionedMatchesRemaining
        ");

        builder.Where("p.IsDeleted = 0");

        if (!string.IsNullOrWhiteSpace(query.Name))
            builder.Where("p.Name LIKE @Name", new { Name = $"%{query.Name}%" });

        if (query.Birthday.HasValue)
            builder.Where("CAST(p.Birthday AS DATE) = CAST(@Birthday AS DATE)", new { query.Birthday });

        if (query.TeamId.HasValue)
            builder.Where("p.TeamId = @TeamId", new { query.TeamId });

        if (query.StatusId.HasValue)
            builder.Where("p.StatusId = @StatusId", new { query.StatusId });

        if (query.SanctionedMatchesRemaining.HasValue)
            builder.Where("p.SanctionedMatchesRemaining = @SanctionedMatchesRemaining", new { query.SanctionedMatchesRemaining });

        var allowedColumns = new[]
            {
            "Name",
            "Birthday",
            "TeamName",
            "StatusName",
            "SanctionedMatchesRemaining",
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

        var data = await multi.ReadAsync<PlayerResponseDto>();
        var total = await multi.ReadSingleAsync<int>();

        return new PagedResult<PlayerResponseDto>(
            data,
            query.PageNumber,
            query.PageSize,
            total);
    }

    public async Task<PlayerResponseDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        const string sql = @"
            SELECT 
                p.Id, 
                p.Name, 
                p.Birthday, 
                t.Name as 'TeamName', 
                ps.Name as 'Status', 
                p.SanctionedMatchesRemaining
            FROM Players p
            INNER JOIN Teams t ON p.TeamId = t.Id
            INNER JOIN PlayerStatuses ps ON p.StatusId = ps.Id
            WHERE p.Id = @Id
              AND p.IsDeleted = 0
        ";

        var player = await connection.QuerySingleOrDefaultAsync<PlayerResponseDto>(
            sql,
            new { Id = id });

        return player;
    }
}
