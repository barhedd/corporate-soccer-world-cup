using CorporateSoccerWorldCup.Application.Common.Pagination;
using CorporateSoccerWorldCup.Application.Features.Teams.Queries.Common.Dtos;
using CorporateSoccerWorldCup.Application.Features.Teams.Queries.Common.Interfaces;
using CorporateSoccerWorldCup.Application.Features.Teams.Queries.GetTeams;
using FluentAssertions;
using Moq;

namespace CorporateSoccerWorldCup.Tests.Application.Teams.Queries;

public class GetTeamsQueryHandlerTests
{
    private readonly Mock<ITeamReadRepository> _teamReadRepositoryMock;
    private readonly GetTeamsQueryHandler _handler;

    public GetTeamsQueryHandlerTests()
    {
        _teamReadRepositoryMock = new Mock<ITeamReadRepository>();
        _handler = new GetTeamsQueryHandler(_teamReadRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnPagedResultWrappedInResultOk()
    {
        // Arrange
        var query = new GetTeamsQuery
        {
            PageNumber = 1,
            PageSize = 10
        };

        var expectedPagedResult = new PagedResult<TeamResponseDto>(
            [
                new() {
                    Id = Guid.NewGuid(),
                    Name = "Barcelona",
                    ImageUrl = "image.png"
                }
            ],
            totalRecords: 1,
            pageNumber: 1,
            pageSize: 10);

        _teamReadRepositoryMock
            .Setup(x => x.GetPagedAsync(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedPagedResult);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Success.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Should().BeEquivalentTo(expectedPagedResult);

        _teamReadRepositoryMock.Verify(
            x => x.GetPagedAsync(query, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyPagedResult_WhenRepositoryReturnsEmpty()
    {
        // Arrange
        var query = new GetTeamsQuery
        {
            PageNumber = 1,
            PageSize = 10
        };

        var emptyPagedResult = new PagedResult<TeamResponseDto>(
            data: [],
            totalRecords: 0,
            pageNumber: 1,
            pageSize: 10);

        _teamReadRepositoryMock
            .Setup(x => x.GetPagedAsync(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(emptyPagedResult);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Success.Should().BeTrue();
        result.Value?.Data.Should().BeEmpty();
        result.Value?.TotalRecords.Should().Be(0);
    }

    [Fact]
    public async Task Handle_ShouldPassCancellationToken_ToRepository()
    {
        // Arrange
        var query = new GetTeamsQuery();
        var cancellationToken = new CancellationTokenSource().Token;

        _teamReadRepositoryMock
            .Setup(x => x.GetPagedAsync(query, cancellationToken))
            .ReturnsAsync(new PagedResult<TeamResponseDto>(
                data: [],
                pageNumber: 0, 
                pageSize: 1, 
                totalRecords: 10));

        // Act
        await _handler.Handle(query, cancellationToken);

        // Assert
        _teamReadRepositoryMock.Verify(
            x => x.GetPagedAsync(query, cancellationToken),
            Times.Once);
    }
}
