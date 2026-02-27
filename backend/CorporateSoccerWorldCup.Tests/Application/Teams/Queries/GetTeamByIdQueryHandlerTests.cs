using CorporateSoccerWorldCup.Application.Common.Errors;
using CorporateSoccerWorldCup.Application.Features.Teams.Queries.Common.Dtos;
using CorporateSoccerWorldCup.Application.Features.Teams.Queries.Common.Interfaces;
using CorporateSoccerWorldCup.Application.Features.Teams.Queries.GetTeamById;
using FluentAssertions;
using Moq;

namespace CorporateSoccerWorldCup.Tests.Application.Teams.Queries;

public class GetTeamByIdQueryHandlerTests
{
    private readonly Mock<ITeamReadRepository> _teamReadRepositoryMock;
    private readonly GetTeamByIdQueryHandler _handler;

    public GetTeamByIdQueryHandlerTests()
    {
        _teamReadRepositoryMock = new Mock<ITeamReadRepository>();
        _handler = new GetTeamByIdQueryHandler(_teamReadRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnOk_WhenTeamExists()
    {
        // Arrange
        var teamId = Guid.NewGuid();
        var query = new GetTeamByIdQuery { Id = teamId };

        var expectedTeam = new TeamResponseDto
        {
            Id = teamId,
            Name = "Manchester City",
            ImageUrl = "city.png"
        };

        _teamReadRepositoryMock
            .Setup(x => x.GetByIdAsync(teamId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedTeam);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Success.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Should().BeEquivalentTo(expectedTeam);

        _teamReadRepositoryMock.Verify(
            x => x.GetByIdAsync(teamId, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFail_WhenTeamDoesNotExist()
    {
        // Arrange
        var teamId = Guid.NewGuid();
        var query = new GetTeamByIdQuery{ Id = teamId };

        _teamReadRepositoryMock
            .Setup(x => x.GetByIdAsync(teamId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((TeamResponseDto?)null);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Success.Should().BeFalse();
        result.Value.Should().BeNull();
        result.ErrorMessage.Should().Be("Team not found");
        result.ErrorCode.Should().Be(ErrorCodes.NotFound);

        _teamReadRepositoryMock.Verify(
            x => x.GetByIdAsync(teamId, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldPassCancellationToken_ToRepository()
    {
        // Arrange
        var teamId = Guid.NewGuid();
        var query = new GetTeamByIdQuery{ Id = teamId };
        var cancellationToken = new CancellationTokenSource().Token;

        _teamReadRepositoryMock
            .Setup(x => x.GetByIdAsync(teamId, cancellationToken))
            .ReturnsAsync(new TeamResponseDto { Id = teamId });

        // Act
        await _handler.Handle(query, cancellationToken);

        // Assert
        _teamReadRepositoryMock.Verify(
            x => x.GetByIdAsync(teamId, cancellationToken),
            Times.Once);
    }
}