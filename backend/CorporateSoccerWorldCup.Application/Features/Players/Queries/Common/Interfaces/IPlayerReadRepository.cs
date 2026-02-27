using CorporateSoccerWorldCup.Application.Common.Pagination;
using CorporateSoccerWorldCup.Application.Features.Players.Queries.Common.Dtos;
using CorporateSoccerWorldCup.Application.Features.Players.Queries.GetPlayers;

namespace CorporateSoccerWorldCup.Application.Features.Players.Queries.Common.Interfaces;

public interface IPlayerReadRepository
{
    Task<PagedResult<PlayerResponseDto>> GetPagedAsync(GetPlayersQuery query, CancellationToken cancellationToken);
    Task<PlayerResponseDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}
