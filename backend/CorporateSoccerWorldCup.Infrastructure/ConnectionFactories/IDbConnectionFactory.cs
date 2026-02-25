using System.Data;

namespace CorporateSoccerWorldCup.Infrastructure.ConnectionFactories;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}
