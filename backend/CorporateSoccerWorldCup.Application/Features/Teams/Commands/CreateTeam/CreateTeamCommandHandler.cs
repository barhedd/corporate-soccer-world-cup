using CorporateSoccerWorldCup.Application.Common.Errors;
using CorporateSoccerWorldCup.Application.Common.Interfaces;
using CorporateSoccerWorldCup.Application.Common.Results;
using CorporateSoccerWorldCup.Domain.Entities;
using CorporateSoccerWorldCup.Domain.Interfaces;
using CorporateSoccerWorldCup.Domain.Interfaces.Repositories;

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

        var team = new Team
        {
            Name = command.Name,
            ImageUrl = command.ImageUrl,
        };

        await _teamRepository.AddAsync(team, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return Result<Guid>.Ok(team.Id);
    }
}
