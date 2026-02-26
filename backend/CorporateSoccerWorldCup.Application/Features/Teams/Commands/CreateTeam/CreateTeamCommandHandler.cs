using CorporateSoccerWorldCup.Application.Abstractions.Events;
using CorporateSoccerWorldCup.Application.Common.Errors;
using CorporateSoccerWorldCup.Application.Common.Results;
using CorporateSoccerWorldCup.Domain.Abstractions;
using CorporateSoccerWorldCup.Domain.Abstractions.Repositories;
using CorporateSoccerWorldCup.Domain.Entities.Teams;

namespace CorporateSoccerWorldCup.Application.Features.Teams.Commands.CreateTeam;

public class CreateTeamCommandHandler(
    ITeamRepository teamRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<CreateTeamCommand, Guid>
{
    private readonly ITeamRepository _teamRepository = teamRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<Guid>> Handle(CreateTeamCommand command, CancellationToken cancellationToken)
    {
        var exists = await _teamRepository.ExistByNameAsync(command.Name, cancellationToken);

        if (exists)
            return Result<Guid>.Fail(
                "Team already exists",
                ErrorCodes.DuplicateError);

        var team = Team.Create(command.Name, command.ImageUrl);

        await _teamRepository.AddAsync(team, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return Result<Guid>.Ok(team.Id);
    }
}
