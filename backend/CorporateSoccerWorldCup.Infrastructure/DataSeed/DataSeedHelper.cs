using CorporateSoccerWorldCup.Domain.Entities;
using CorporateSoccerWorldCup.Domain.Entities.MatchStatuses;
using CorporateSoccerWorldCup.Domain.Entities.PlayerStatuses;
using CorporateSoccerWorldCup.Domain.Entities.Teams;
using CorporateSoccerWorldCup.Infrastructure.Contexts;
using System.Text.Json;

namespace CorporateSoccerWorldCup.Infrastructure.DataSeed;

public static class DataSeedHelper
{
    private static List<T> LoadJson<T>(string fileName)
    {
        var path = Path.Combine(AppContext.BaseDirectory, "DataSeed", fileName);
        var json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<List<T>>(json) ?? [];
    }

    public static async Task SeedAsync(this CorporateSoccerWorldCupContext dbContext)
    {
        // --- MatchStatuses ---
        if (!dbContext.MatchStatuses.Any())
        {
            var matchStatuses = LoadJson<MatchStatus>("matchStatuses.json");
            await dbContext.MatchStatuses.AddRangeAsync(matchStatuses);
            await dbContext.SaveChangesAsync();
        }

        // --- PlayerStatuses ---
        if (!dbContext.PlayerStatuses.Any())
        {
            var playerStatuses = LoadJson<PlayerStatus>("playerStatuses.json");
            await dbContext.PlayerStatuses.AddRangeAsync(playerStatuses);
            await dbContext.SaveChangesAsync();
        }

        // --- Teams ---
        if (!dbContext.Teams.Any())
        {
            var teams = LoadJson<Team>("teams.json");
            await dbContext.Teams.AddRangeAsync(teams);
            await dbContext.SaveChangesAsync();
        }

        // --- Players ---
        if (!dbContext.Players.Any())
        {
            var players = LoadJson<Player>("players.json");
            await dbContext.Players.AddRangeAsync(players);
            await dbContext.SaveChangesAsync();
        }
    }
}
