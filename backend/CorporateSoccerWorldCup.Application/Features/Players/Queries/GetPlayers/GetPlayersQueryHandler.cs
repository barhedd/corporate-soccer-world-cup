using CorporateSoccerWorldCup.Application.Common.Abstractions.Events;
using CorporateSoccerWorldCup.Application.Common.Pagination;
using CorporateSoccerWorldCup.Application.Common.Results;
using CorporateSoccerWorldCup.Application.Features.Players.Queries.Common.Dtos;
using CorporateSoccerWorldCup.Application.Features.Players.Queries.Common.Interfaces;
using CorporateSoccerWorldCup.Application.Features.Teams.Queries.GetTeams;

namespace CorporateSoccerWorldCup.Application.Features.Players.Queries.GetPlayers;

public class GetPlayersQueryHandler(
    IPlayerReadRepository playerReadRepository) : IQueryHandler<GetPlayersQuery, PagedResult<PlayerResponseDto>>
{
    private readonly IPlayerReadRepository _playerReadRepository = playerReadRepository;

    public async Task<Result<PagedResult<PlayerResponseDto>>> Handle(GetPlayersQuery query, CancellationToken cancellationToken)
    {
        var pagedResult = await _playerReadRepository.GetPagedAsync(query, cancellationToken);

        return Result<PagedResult<PlayerResponseDto>>.Ok(pagedResult);
    }
}
