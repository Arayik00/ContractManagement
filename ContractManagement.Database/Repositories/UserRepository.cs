using ContractManagement.Database.Interfaces;
using ContractManagement.Model.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractManagement.Database.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;
        public UserRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<Users>? GetById(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("SELECT * FROM Users WHERE Id=@id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            await conn.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();
            if (!await reader.ReadAsync()) return null;

            return new Users
            {
                Id = (int)reader["Id"],
                FirstName = (string)reader["FirstName"],
                LastName = (string)reader["LastName"],
                Username = (string)reader["Username"],
                Password = (string)reader["Password"]
            };
        }

        public async Task<List<Users>> GetAll()
        {
            var list = new List<Users>();
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("SELECT * FROM Users", conn);
            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new Users
                {
                    Id = (int)reader["Id"],
                    FirstName = (string)reader["FirstName"],
                    LastName = (string)reader["LastName"],
                    Username = (string)reader["Username"],
                    Password = (string)reader["Password"]
                });
            }

            return list;
        }

        public async Task<Users?> GetByCredentialsAsync(string username, string password)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new SqlCommand(
                "SELECT Id, FirstName FROM Users WHERE Username=@u AND Password=@p", conn);

            cmd.Parameters.AddWithValue("@u", username);
            cmd.Parameters.AddWithValue("@p", password);

            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Users
                {
                    Id = reader.GetInt32(0),
                    FirstName = reader.GetString(1)
                };
            }

            return null;
        }

        public async Task<Users?> GetUserByUsername(string username)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new SqlCommand(
                "SELECT Id, Username FROM Users WHERE Username=@u", conn);

            cmd.Parameters.AddWithValue("@u", username);

            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Users
                {
                    Id = reader.GetInt32(0),
                    Username = reader.GetString(1)
                };
            }

            return null;
        }

        public async Task<bool> CreateUserAsync(Users user)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                await conn.OpenAsync();

                using var cmd = new SqlCommand(
                    "INSERT INTO Users (FirstName, LastName, Username, Password) VALUES (@firstName, @lastName, @username, @password)",
                    conn);

                cmd.Parameters.AddWithValue("@firstName", user.FirstName);
                cmd.Parameters.AddWithValue("@lastName", user.LastName);
                cmd.Parameters.AddWithValue("@username", user.Username);
                cmd.Parameters.AddWithValue("@password", user.Password);

                var rowsAffected = await cmd.ExecuteNonQueryAsync();
                return rowsAffected > 0; // true if insert succeeded
            }
            catch
            {
                // log exception if needed
                return false;
            }
        }

        public async Task<bool> UserHasCompanyAsync(int userId)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new SqlCommand("SELECT COUNT(1) FROM Companies WHERE adminId=@userId", conn);
            cmd.Parameters.AddWithValue("@userId", userId);

            var count = (int)await cmd.ExecuteScalarAsync();
            return count > 0;
        }

        public async Task<List<Contracts>> GetContractsByUserIdAsync(int userId)
        {
            var list = new List<Contracts>();
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new SqlCommand(
                "SELECT Id, CompanyId, UserId, Position, Description, StartDate, EndDate, Wage " +
                "FROM Contracts WHERE UserId=@userId",
                conn);
            cmd.Parameters.AddWithValue("@userId", userId);

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new Contracts
                {
                    Id = reader.GetInt32(0),
                    CompanyId = reader.GetInt32(1),
                    UserId = reader.GetInt32(2),
                    Position = reader.GetString(3),
                    Description = reader.GetString(4),
                    StartDate = reader.GetDateTime(5),
                    EndDate = reader.GetDateTime(6),
                    Wage = reader.GetDecimal(7)
                });
            }
            return list;
        }

    }
}
