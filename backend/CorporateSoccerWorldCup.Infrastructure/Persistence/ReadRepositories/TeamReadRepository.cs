using CorporateSoccerWorldCup.Application.Features.Teams.DataTransferObjects;
using CorporateSoccerWorldCup.Application.Features.Teams.Interfaces;
using CorporateSoccerWorldCup.Infrastructure.ConnectionFactories;
using Dapper;

namespace CorporateSoccerWorldCup.Infrastructure.Persistence.ReadRepositories;

public class TeamReadRepository(IDbConnectionFactory dbConnectionFactory) : ITeamReadRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory = dbConnectionFactory;

    public async Task<IEnumerable<TeamDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        var sql = """
            SELECT *
            FROM Teams
            WHERE IsDeleted = 0
        """;

        return await connection.QueryAsync<TeamDto>(sql);
    }

    public Task<TeamDto> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
