using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Orders.Api.Services.Dapper;

public class DapperContext
{
    private readonly IConfiguration _configuration;

    public DapperContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IDbConnection CreateConnection(string connectionName)
        => new SqlConnection(_configuration[$"ConnectionStrings:{connectionName}"]);
}