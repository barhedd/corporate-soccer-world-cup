using CorporateSoccerWorldCup.Application.Interfaces;
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

    public async Task<Guid> Handle(CreateTeamCommand command, CancellationToken cancellationToken)
    {
        var team = new Team
        {
            Name = command.Name,
            ImageUrl = command.ImageUrl,
        };

        await _teamRepository.AddAsync(team, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return team.Id;
    }
}
