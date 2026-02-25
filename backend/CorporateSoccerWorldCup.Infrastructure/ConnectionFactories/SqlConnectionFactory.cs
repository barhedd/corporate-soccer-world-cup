using Microsoft.Data.SqlClient;
using System.Data;

namespace CorporateSoccerWorldCup.Infrastructure.ConnectionFactories;

public class SqlConnectionFactory(string connectionString) : IDbConnectionFactory
{
    private readonly string _connectionString = connectionString;
    public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
}
