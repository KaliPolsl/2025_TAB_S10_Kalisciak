using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;
//using AquaparkApp.Models;

public class UserService
{
    private readonly string _connectionString;

    public UserService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("AzureSqlConnection");
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        using IDbConnection db = new SqlConnection(_connectionString);

        var query = "SELECT UserId, Username, Email, Role, IsBlocked, CreatedAt, UpdatedAt FROM dbo.Users";
        return await db.QueryAsync<User>(query);
    }
}