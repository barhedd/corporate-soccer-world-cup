using CorporateSoccerWorldCup.Api.Commons.Extensions;
using CorporateSoccerWorldCup.Api.Controllers.Players.Contracts;
using CorporateSoccerWorldCup.Application.Common.Abstractions.Events;
using CorporateSoccerWorldCup.Application.Common.Pagination;
using CorporateSoccerWorldCup.Application.Features.Players.Commands.CreatePlayer;
using CorporateSoccerWorldCup.Application.Features.Players.Queries.Common.Dtos;
using CorporateSoccerWorldCup.Application.Features.Players.Queries.GetPlayerById;
using CorporateSoccerWorldCup.Application.Features.Players.Queries.GetPlayers;
using Microsoft.AspNetCore.Mvc;

namespace CorporateSoccerWorldCup.Api.Controllers.Players;

[ApiController]
[Route("api/[controller]")]
public class PlayersController(
    ICommandHandler<CreatePlayerCommand, Guid> createPlayerHandler,
    IQueryHandler<GetPlayersQuery, PagedResult<PlayerResponseDto>> getPlayersHandler,
    IQueryHandler<GetPlayerByIdQuery, PlayerResponseDto> getPlayerByIdHandler) : ControllerBase
{
    private readonly ICommandHandler<CreatePlayerCommand, Guid> _createPlayerHandler = createPlayerHandler;
    IQueryHandler<GetPlayersQuery, PagedResult<PlayerResponseDto>> _getPlayersHandler = getPlayersHandler;
    private readonly IQueryHandler<GetPlayerByIdQuery, PlayerResponseDto> _getPlayerByIdHandler = getPlayerByIdHandler;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePlayerRequest request)
    {
        var command = new CreatePlayerCommand
        {
            Name = request.Name,
            Birthday = request.Birthday,
            TeamId = request.TeamId,
            StatusId = request.StatusId,
            SanctionedMatchesRemaining = request.SanctionedMatchesRemaining
        };

        var result = await _createPlayerHandler.Handle(command, HttpContext.RequestAborted);

        if (result.Failure)
            return result.ToFailureResult();

        return CreatedAtAction(nameof(GetById), new { id = result.Value }, result.Value);
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] GetPlayersRequest request)
    {
        var query = new GetPlayersQuery
        {
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            SortBy = request.SortBy,
            SortDirection = request.SortDirection,
            Name = request.Name,
            Birthday = request.Birthday,
            TeamId = request.TeamId,
            StatusId = request.StatusId,
            SanctionedMatchesRemaining = request.SanctionedMatchesRemaining
        };

        var result = await _getPlayersHandler.Handle(query, HttpContext.RequestAborted);

        if (result.Failure)
            return result.ToFailureResult();

        return Ok(result.Value);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = new GetPlayerByIdQuery
        {
            Id = id
        };

        var result = await _getPlayerByIdHandler.Handle(
            query,
            HttpContext.RequestAborted);

        if (result.Failure)
            return result.ToFailureResult();

        return Ok(result.Value);
    }
}
