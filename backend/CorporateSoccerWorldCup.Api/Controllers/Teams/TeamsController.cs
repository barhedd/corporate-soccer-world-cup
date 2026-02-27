using CorporateSoccerWorldCup.Api.Commons.Extensions;
using CorporateSoccerWorldCup.Api.Controllers.Teams.Contracts;
using CorporateSoccerWorldCup.Application.Common.Abstractions.Events;
using CorporateSoccerWorldCup.Application.Common.Pagination;
using CorporateSoccerWorldCup.Application.Features.Teams.Commands.CreateTeam;
using CorporateSoccerWorldCup.Application.Features.Teams.Commands.DeleteTeam;
using CorporateSoccerWorldCup.Application.Features.Teams.Commands.UpdateTeam;
using CorporateSoccerWorldCup.Application.Features.Teams.Queries.Common.Dtos;
using CorporateSoccerWorldCup.Application.Features.Teams.Queries.GetTeamById;
using CorporateSoccerWorldCup.Application.Features.Teams.Queries.GetTeams;
using Microsoft.AspNetCore.Mvc;

namespace CorporateSoccerWorldCup.Api.Controllers.Teams;

[ApiController]
[Route("api/[controller]")]
public class TeamsController(
    ICommandHandler<CreateTeamCommand, Guid> createTeamHandler,
    IQueryHandler<GetTeamsQuery, PagedResult<TeamResponseDto>> getTeamsHandler,
    IQueryHandler<GetTeamByIdQuery, TeamResponseDto> getTeamByIdHandler,
    ICommandHandler<UpdateTeamCommand, TeamResponseDto> updateTeamHandler,
    ICommandHandler<DeleteTeamCommand> deleteTeamHandler) : ControllerBase
{
    private readonly ICommandHandler<CreateTeamCommand, Guid> _createTeamHandler = createTeamHandler;
    private readonly IQueryHandler<GetTeamsQuery, PagedResult<TeamResponseDto>> _getTeamsHandler = getTeamsHandler;
    private readonly IQueryHandler<GetTeamByIdQuery, TeamResponseDto> _getTeamByIdHandler = getTeamByIdHandler;
    private readonly ICommandHandler<UpdateTeamCommand, TeamResponseDto> _updateTeamHandler = updateTeamHandler;
    private readonly ICommandHandler<DeleteTeamCommand> _deleteTeamHandler = deleteTeamHandler;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTeamRequest request)
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
    public async Task<IActionResult> Get([FromQuery] GetTeamsRequest request)
    {
        var query = new GetTeamsQuery
        {
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            SortBy = request.SortBy,
            SortDirection = request.SortDirection,
            Name = request.Name,
        };

        var result = await _getTeamsHandler.Handle(
            query,
            HttpContext.RequestAborted);

        if (result.Failure)
            return result.ToFailureResult(); 

        return Ok(result.Value);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = new GetTeamByIdQuery
        {
            Id = id
        };

        var result = await _getTeamByIdHandler.Handle(
            query,
            HttpContext.RequestAborted);

        if (result.Failure)
            return result.ToFailureResult();

        return Ok(result.Value);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTeamRequest request)
    {
        var command = new UpdateTeamCommand
        {
            Id = id,
            Name = request.Name,
            ImageUrl = request.ImageUrl
        };

        var result = await _updateTeamHandler.Handle(command, HttpContext.RequestAborted);

        if (result.Failure)
            return result.ToFailureResult();

        return Ok(result.Value);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteTeamCommand { Id = id };

        var result = await _deleteTeamHandler.Handle(command, HttpContext.RequestAborted);

        if (result.Failure)
            return result.ToFailureResult();

        return Ok();
    }
}