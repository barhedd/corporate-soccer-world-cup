using CorporateSoccerWorldCup.Api.Commons.Extensions;
using CorporateSoccerWorldCup.Api.Contracts.Requests;
using CorporateSoccerWorldCup.Application.Common.Interfaces;
using CorporateSoccerWorldCup.Application.Features.Teams.Commands.CreateTeam;
using CorporateSoccerWorldCup.Application.Features.Teams.DataTransferObjects;
using CorporateSoccerWorldCup.Application.Features.Teams.Queries.GetTeams;
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
    public async Task<IActionResult> Create(CreateTeamRequest request)
    {
        var command = new CreateTeamCommand
        {
            Name = request.Name,
            ImageUrl = request.ImageUrl,
        };

        var result = await _createTeamHandler.Handle(
            command,
            HttpContext.RequestAborted);

        if (result.Failure)
            return result.ToFailureResult();

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.Value },
            new { id = result.Value });
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _getTeamsHandler.Handle(
            new GetTeamsQuery(),
            HttpContext.RequestAborted);

        if (result.Failure)
            return result.ToFailureResult(); 

        return Ok(result.Value);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(Guid id)
    {
        return Ok();
    }
}
