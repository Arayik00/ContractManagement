using ContractManagement.Database.Interfaces;
using ContractManagement.Model.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractManagement.Database.Repositories
{
    public class ContractRepository : IContractRepository
    {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;
        public ContractRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<Contracts>? GetById(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("SELECT * FROM Contracts WHERE Id=@id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            await conn.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();
            if (!await reader.ReadAsync()) return null;

            return new Contracts
            {
                Id = (int)reader["Id"],
                CompanyId = (int)reader["CompanyId"],
                UserId = (int)reader["UserId"],
                Position = (string)reader["Position"],
                Description = (string)reader["Description"],
                StartDate = (DateTime)reader["StartDate"],
                EndDate = (DateTime)reader["EndDate"],
                Wage = (decimal)reader["Wage"]
            };
        }
        public async Task<List<Contracts>> GetAll()
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("SELECT * FROM Contracts", conn);
            List<Contracts> contracts = new List<Contracts>();
            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                contracts.Add(new Contracts
                {
                    Id = (int)reader["Id"],
                    CompanyId = (int)reader["CompanyId"],
                    UserId = (int)reader["UserId"],
                    Position = (string)reader["Position"],
                    Description = (string)reader["Description"],
                    StartDate = (DateTime)reader["StartDate"],
                    EndDate = (DateTime)reader["EndDate"],
                    Wage = (decimal)reader["Wage"]
                });
            }
            return contracts;
        }

        public async Task<List<ContractNames>> GetAllCompanyContracts(int companyId)
        {
            var list = new List<ContractNames>();
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new SqlCommand(
              @"SELECT c.Id, c.CompanyId, c.Position, c.Description, c.StartDate, c.EndDate, c.Wage,
                 u.FirstName, u.LastName
          FROM Contracts c
          INNER JOIN Users u ON c.UserId = u.Id
          WHERE c.CompanyId = @companyId",
              conn);

            cmd.Parameters.AddWithValue("@companyId", companyId);

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                Contracts contract = new Contracts
                {
                    Id = reader.GetInt32(0),
                    CompanyId = reader.GetInt32(1),
                    Position = reader.GetString(2),
                    Description = reader.GetString(3),
                    StartDate = reader.GetDateTime(4),
                    EndDate = reader.GetDateTime(5),
                    Wage = reader.GetDecimal(6),
                };
                string firstName = reader.GetString(7);
                string lastName = reader.GetString(8);
                list.Add(new ContractNames
                {
                    contract = contract,
                    firstName = firstName,
                    lastName = lastName,
                });
            }
            return list;
        }
        public async Task<bool> InsertContractAsync(Contracts contract)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new SqlCommand(
                "INSERT INTO Contracts (CompanyId, UserId, Position, Description, StartDate, EndDate, Wage) " +
                "VALUES (@companyId, @userId, @position, @description, @start, @end, @wage)",
                conn);

            cmd.Parameters.AddWithValue("@companyId", contract.CompanyId);
            cmd.Parameters.AddWithValue("@userId", contract.UserId);
            cmd.Parameters.AddWithValue("@position", contract.Position);
            cmd.Parameters.AddWithValue("@description", contract.Description);
            cmd.Parameters.AddWithValue("@start", contract.StartDate);
            cmd.Parameters.AddWithValue("@end", contract.EndDate);
            cmd.Parameters.AddWithValue("@wage", contract.Wage);

            await cmd.ExecuteNonQueryAsync();

            // Insert user role
            cmd = new SqlCommand(
                "INSERT INTO UserRoles (UserId, CompanyId, Role) VALUES (@userId, @companyId, @role)",
                conn);

            cmd.Parameters.AddWithValue("@userId", contract.UserId);
            cmd.Parameters.AddWithValue("@companyId", contract.CompanyId);
            cmd.Parameters.AddWithValue("@role", "user");

            int rowsAffected = await cmd.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteContractAsync(int ContractId)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new SqlCommand(
              @"DELETE FROM Contracts WHERE Id=@contractId;",
              conn);
            cmd.Parameters.AddWithValue("@contractId", ContractId);
            int rowsAffected = await cmd.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> UpdateContractData(Contracts updatedContract)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            var sql = @"
            UPDATE Contracts
            SET 
                Position = @Position,
                Description = @Description,
                StartDate = @StartDate,
                EndDate = @EndDate,
                Wage = @Wage
            WHERE Id = @ContractId";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Position", updatedContract.Position ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Description", updatedContract.Description ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@StartDate", updatedContract.StartDate);
            cmd.Parameters.AddWithValue("@EndDate", updatedContract.EndDate);
            cmd.Parameters.AddWithValue("@Wage", updatedContract.Wage);
            cmd.Parameters.AddWithValue("@ContractId", updatedContract.Id);
            int rowsAffected = await cmd.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> CanUserEditContractAsync(int contractId, int userId)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new SqlCommand(@"
                SELECT c.CompanyId, co.AdminId
                FROM Contracts c
                JOIN Companies co ON c.CompanyId = co.Id
                WHERE c.Id = @cid;
            ", conn);

            cmd.Parameters.AddWithValue("@cid", contractId);

            using var reader = await cmd.ExecuteReaderAsync();
            if (!reader.Read()) return false;

            int companyAdminId = (int)reader["AdminId"];

            return userId == companyAdminId;
        }
    }
}
