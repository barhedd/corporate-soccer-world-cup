using CorporateSoccerWorldCup.Domain.Abstractions;
using CorporateSoccerWorldCup.Infrastructure.Contexts;

namespace CorporateSoccerWorldCup.Infrastructure.Persistence;

public class UnitOfWork(CorporateSoccerWorldCupContext dbContext) : IUnitOfWork
{
    private readonly CorporateSoccerWorldCupContext _dbContext = dbContext;

    public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
