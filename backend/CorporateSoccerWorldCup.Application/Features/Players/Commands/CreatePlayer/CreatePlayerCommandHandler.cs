using CorporateSoccerWorldCup.Application.Common.Abstractions.Events;
using CorporateSoccerWorldCup.Application.Common.Errors;
using CorporateSoccerWorldCup.Application.Common.Results;
using CorporateSoccerWorldCup.Domain.Abstractions;
using CorporateSoccerWorldCup.Domain.Abstractions.Repositories;
using CorporateSoccerWorldCup.Domain.Entities;
using CorporateSoccerWorldCup.Domain.Entities.PlayerStatuses;

namespace CorporateSoccerWorldCup.Application.Features.Players.Commands.CreatePlayer;

public class CreatePlayerCommandHandler(
    IPlayerRepository playerRepository,
    IPlayerStatusRepository playerStatusRepository,
    ITeamRepository teamRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<CreatePlayerCommand, Guid>
{
    private readonly IPlayerRepository _playerRepository = playerRepository;
    private readonly IPlayerStatusRepository _playerStatusRepository = playerStatusRepository;
    private readonly ITeamRepository _teamRepository = teamRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<Guid>> Handle(CreatePlayerCommand command, CancellationToken cancellationToken)
    {
        var team = await _teamRepository.GetByIdAsync(command.TeamId, cancellationToken);

        if (team is null)
            return Result<Guid>.Fail(
                "Team not found",
                ErrorCodes.NotFound);

        var status = await _playerStatusRepository.GetByIdAsync(command.StatusId, cancellationToken);   

        if (status is null)
            return Result<Guid>.Fail(
                "Status not found",
                ErrorCodes.NotFound);

        if (status.Status == PlayerStatusEnum.Sanctioned)
            if (!command.SanctionedMatchesRemaining.HasValue || command.SanctionedMatchesRemaining <= 0)
                return Result<Guid>.Fail(
                    "Sanctioned players musth have a positive number of sanctioned matches",
                    ErrorCodes.InvalidRange);

        var player = Player.Create(
            command.Name, 
            command.Birthday, 
            command.TeamId, 
            command.StatusId,
            status.Status == PlayerStatusEnum.Sanctioned
                                        ? command.SanctionedMatchesRemaining
                                        : null);

        await _playerRepository.AddAsync(player, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return Result<Guid>.Ok(player.Id);
    }
}
