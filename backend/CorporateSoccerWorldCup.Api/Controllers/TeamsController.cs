using CorporateSoccerWorldCup.Application.Features.Teams.Commands.CreateTeam;
using CorporateSoccerWorldCup.Application.Features.Teams.DataTransferObjects;
using CorporateSoccerWorldCup.Application.Features.Teams.Queries.GetTeams;
using CorporateSoccerWorldCup.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CorporateSoccerWorldCup.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeamsController(
    ICommandHandler<CreateTeamCommand, Guid> createTeamHandler,
    IQueryHandler<GetTeamsQuery, IEnumerable<TeamDto>> getTeamsHandler) : ControllerBase
{
    private readonly ICommandHandler<CreateTeamCommand, Guid> _createTeamHandler = createTeamHandler;
    private readonly IQueryHandler<GetTeamsQuery, IEnumerable<TeamDto>> _getTeamsHandler = getTeamsHandler;

    [HttpPost]
    public async Task<IActionResult> Create(CreateTeamCommand command)
    {
        var id = await _createTeamHandler.Handle(
            command,
            HttpContext.RequestAborted);

        return Ok(id);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _getTeamsHandler.Handle(
            new GetTeamsQuery(),
            HttpContext.RequestAborted);

        return Ok(result);
    }
}
