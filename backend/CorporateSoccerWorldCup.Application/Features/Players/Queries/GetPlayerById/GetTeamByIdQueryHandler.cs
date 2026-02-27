using CorporateSoccerWorldCup.Application.Common.Abstractions.Events;
using CorporateSoccerWorldCup.Application.Common.Errors;
using CorporateSoccerWorldCup.Application.Common.Results;
using CorporateSoccerWorldCup.Application.Features.Players.Queries.Common.Dtos;
using CorporateSoccerWorldCup.Application.Features.Players.Queries.Common.Interfaces;

namespace CorporateSoccerWorldCup.Application.Features.Players.Queries.GetPlayerById;

public class GetTeamByIdQueryHandler(
    IPlayerReadRepository playerReadRepository) : IQueryHandler<GetPlayerByIdQuery, PlayerResponseDto>
{
    private readonly IPlayerReadRepository _playerReadRepository = playerReadRepository;
    
    public async Task<Result<PlayerResponseDto>> Handle(GetPlayerByIdQuery query, CancellationToken cancellationToken)
    {
        var player = await _playerReadRepository.GetByIdAsync(query.Id, cancellationToken);

        if (player is null)
            return Result<PlayerResponseDto>.Fail(
                "Player not found",
                ErrorCodes.NotFound);

        return Result<PlayerResponseDto>.Ok(player);
    }
}