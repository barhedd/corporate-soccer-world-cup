using CorporateSoccerWorldCup.Application.Common.Abstractions.Events;
using CorporateSoccerWorldCup.Application.Common.Errors;
using CorporateSoccerWorldCup.Application.Common.Results;
using CorporateSoccerWorldCup.Domain.Abstractions;
using CorporateSoccerWorldCup.Domain.Abstractions.Repositories;

namespace CorporateSoccerWorldCup.Application.Features.Teams.Commands.DeleteTeam;

public class DeleteTeamCommandHandler(
    ITeamRepository teamRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<DeleteTeamCommand>
{
    private readonly ITeamRepository _teamRepository = teamRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(DeleteTeamCommand command, CancellationToken cancellationToken)
    {
        var team = await _teamRepository.GetByIdAsync(command.Id, cancellationToken);

        if (team is null)
            return Result.Fail(
                "Team does not exist",
                ErrorCodes.NotFound);

        _teamRepository.Delete(team);
        await _unitOfWork.CommitAsync(cancellationToken);

        return Result.Ok();
    }
}
