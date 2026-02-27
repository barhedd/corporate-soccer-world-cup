using CorporateSoccerWorldCup.Application.Abstractions.Events;
using CorporateSoccerWorldCup.Application.Common.Errors;
using CorporateSoccerWorldCup.Application.Common.Results;
using CorporateSoccerWorldCup.Application.Features.Teams.Queries.Common.Dtos;
using CorporateSoccerWorldCup.Domain.Abstractions;
using CorporateSoccerWorldCup.Domain.Abstractions.Repositories;

namespace CorporateSoccerWorldCup.Application.Features.Teams.Commands.UpdateTeam;

public class UpdateTeamCommandHandler(
    ITeamRepository teamRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<UpdateTeamCommand, TeamResponseDto>
{
    private readonly ITeamRepository _teamRepository = teamRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<TeamResponseDto>> Handle(UpdateTeamCommand command, CancellationToken cancellationToken)
    {
        var team = await _teamRepository.GetByIdAsync(command.Id, cancellationToken);

        if (team is null)
            return Result<TeamResponseDto>.Fail(
                "Team does not exist",
                ErrorCodes.NotFound);

        // Update properties
        team.Name = command.Name;
        team.ImageUrl = command.ImageUrl;
        team.SetEdited("SYSTEM", DateTimeOffset.Now);

        await _unitOfWork.CommitAsync(cancellationToken);

        var updatedTeam = new TeamResponseDto
        {
            Id = command.Id,
            Name = command.Name,
            ImageUrl = command.ImageUrl,
        };

        return Result<TeamResponseDto>.Ok(updatedTeam);
    }
}