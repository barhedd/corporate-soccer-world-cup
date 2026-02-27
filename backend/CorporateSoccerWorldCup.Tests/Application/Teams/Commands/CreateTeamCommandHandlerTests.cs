using CorporateSoccerWorldCup.Application.Common.Errors;
using CorporateSoccerWorldCup.Application.Features.Teams.Commands.CreateTeam;
using CorporateSoccerWorldCup.Domain.Abstractions;
using CorporateSoccerWorldCup.Domain.Abstractions.Repositories;
using CorporateSoccerWorldCup.Domain.Entities.Teams;
using FluentAssertions;
using Moq;

namespace CorporateSoccerWorldCup.Tests.Application.Teams.Commands;

public class CreateTeamCommandHandlerTests
{
    private readonly Mock<ITeamRepository> _teamRepoMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly CreateTeamCommandHandler _handler;

    public CreateTeamCommandHandlerTests()
    {
        _teamRepoMock = new Mock<ITeamRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new CreateTeamCommandHandler(
            _teamRepoMock.Object,
            _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnFail_WhenTeamAlreadyExists()
    {
        // Arrange
        var command = new CreateTeamCommand { Name = "Barcelona", ImageUrl = "image.png" };

        _teamRepoMock
            .Setup(x => x.ExistByNameAsync(command.Name, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Success.Should().BeFalse();
        result.ErrorMessage.Should().Be("Team already exists");
        result.ErrorCode.Should().Be(ErrorCodes.DuplicateError);

        _teamRepoMock.Verify(
            x => x.AddAsync(It.IsAny<Team>(), It.IsAny<CancellationToken>()),
            Times.Never);

        _unitOfWorkMock.Verify(
            x => x.CommitAsync(It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldCreateTeamAndCommit_WhenTeamDoesNotExist()
    {
        // Arrange
        var command = new CreateTeamCommand { Name = "Real Madrid", ImageUrl = "image.png" };

        _teamRepoMock
            .Setup(x => x.ExistByNameAsync(command.Name, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _unitOfWorkMock
            .Setup(x => x.CommitAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Success.Should().BeTrue();
        result.Value.Should().NotBeEmpty();

        _teamRepoMock.Verify(
            x => x.AddAsync(It.IsAny<Team>(), It.IsAny<CancellationToken>()),
            Times.Once);

        _unitOfWorkMock.Verify(
            x => x.CommitAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldPassCorrectTeamData_ToRepository()
    {
        // Arrange
        var command = new CreateTeamCommand { Name = "Arsenal", ImageUrl = "arsenal.png" };

        _teamRepoMock
            .Setup(x => x.ExistByNameAsync(command.Name, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        Team? capturedTeam = null;

        _teamRepoMock
            .Setup(x => x.AddAsync(It.IsAny<Team>(), It.IsAny<CancellationToken>()))
            .Callback<Team, CancellationToken>((team, _) =>
            {
                capturedTeam = team;
            })
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        capturedTeam.Should().NotBeNull();
        capturedTeam!.Name.Should().Be("Arsenal");
        capturedTeam.ImageUrl.Should().Be("arsenal.png");
    }
}
