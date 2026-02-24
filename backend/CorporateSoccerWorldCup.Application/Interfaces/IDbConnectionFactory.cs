using System.Data;

namespace CorporateSoccerWorldCup.Application.Interfaces;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}
