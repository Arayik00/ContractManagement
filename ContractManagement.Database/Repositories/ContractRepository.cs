using ContractManagement.BL.Entities;
using ContractManagement.BL.Interfaces;
using ContractManagement.Model.DTO;
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

        public async Task<ContractDto>? GetById(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("Select c.Id, u.FirstName, u.LastName, c.Description, c.Position,c.Wage, c.StartDate, c.EndDate, c.UserId " +
                "From Contracts c join Users u on c.UserId = u.Id where c.Id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            await conn.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();
            if (!await reader.ReadAsync()) return null;

            return new ContractDto
            {
                Id = (int)reader["Id"],
                Position = (string)reader["Position"],
                Description = (string)reader["Description"],
                StartDate = (DateTime)reader["StartDate"],
                EndDate = (DateTime)reader["EndDate"],
                Wage = (decimal)reader["Wage"],
                User = new UserDto
                {
                    Id = (int)reader["UserId"],
                    FirstName = (string)reader["FirstName"],
                    LastName = (string)reader["LastName"]
                }
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

        public async Task<List<ContractDto>> GetAllCompanyContracts(int companyId)
        {
            var list = new List<ContractDto>();
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
                ContractDto contract = new ContractDto
                {
                    Id = reader.GetInt32(0),
                    Position = reader.GetString(2),
                    Description = reader.GetString(3),
                    StartDate = reader.GetDateTime(4),
                    EndDate = reader.GetDateTime(5),
                    Wage = reader.GetDecimal(6),
                    User = new UserDto
                    {
                        FirstName = reader.GetString(7),
                        LastName = reader.GetString(8),
                    }
                };
                
                list.Add(contract);
            }
            return list;
        }

        public async Task<FilterContractDto> GetAllCompanyContracts(int companyId, UserFilter filter)
        {
            // sanity check
            if (filter.pageNumber <= 0)
                filter.pageNumber = 1;
            string[] statuses = new[] { "All", "Active", "Finished", "NotStartedYet" };

            Console.WriteLine(filter.contractStatus);

            if (!statuses.Contains(filter.contractStatus))
                filter.contractStatus = "All";
            if (filter.pageSize <= 0)
                filter.pageSize = 10;
            var list = new List<ContractDto>();
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new SqlCommand(
              @"SELECT c.Id, c.CompanyId, c.Position, c.Description, c.StartDate, c.EndDate, c.Wage,
                 u.FirstName, u.LastName
          FROM Contracts c
          INNER JOIN Users u ON c.UserId = u.Id
          WHERE c.CompanyId = @companyId
          AND
          (
                @searchString IS NULL
                OR LTRIM(RTRIM(@searchString)) = ''
                OR c.Position LIKE '%' + @searchString + '%'
                OR c.Description LIKE '%' + @searchString + '%'
                OR u.FirstName LIKE '%' + @searchString + '%'
                OR u.LastName LIKE '%' + @searchString + '%'
          )
          AND
          (
                @selectedStatus = 'All'
                OR (@selectedStatus = 'Active' AND c.StartDate <= CAST(GETDATE() AS DATE) AND c.EndDate >= CAST(GETDATE() AS DATE))
                OR (@selectedStatus = 'Finished' AND c.EndDate < CAST(GETDATE() AS DATE))
                OR (@selectedStatus = 'NotStartedYet' AND c.StartDate > CAST(GETDATE() AS DATE))
          )
          ORDER BY c.Id
          OFFSET @startingFrom ROWS
          FETCH NEXT @count ROWS ONLY
          ",
              conn);

            cmd.Parameters.AddWithValue("@companyId", companyId);
            cmd.Parameters.AddWithValue("@startingFrom", (filter.pageNumber - 1) * filter.pageSize);
            cmd.Parameters.AddWithValue("@count", filter.pageSize);
            cmd.Parameters.AddWithValue("@searchString", (object?)filter.searchKey ?? DBNull.Value);
            Console.WriteLine(filter.contractStatus);
            cmd.Parameters.AddWithValue("@selectedStatus", filter.contractStatus);

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                ContractDto contract = new ContractDto
                {
                    Id = reader.GetInt32(0),
                    Position = reader.GetString(2),
                    Description = reader.GetString(3),
                    StartDate = reader.GetDateTime(4),
                    EndDate = reader.GetDateTime(5),
                    Wage = reader.GetDecimal(6),
                    User = new UserDto
                    {
                        FirstName = reader.GetString(7),
                        LastName = reader.GetString(8),
                    }
                };

                list.Add(contract);
            }

            cmd = new SqlCommand(@"SELECT COUNT(*) 
                        FROM Contracts c
                        INNER JOIN Users u ON c.UserId = u.Id
                        WHERE c.CompanyId = @companyId;", conn);
            cmd.Parameters.AddWithValue("@companyId", companyId);
            int totalCount = (int)await cmd.ExecuteScalarAsync();
            FilterContractDto c = new FilterContractDto
            {
                hasNextPage = totalCount > filter.pageSize * filter.pageNumber,
                hasPreviousPage = filter.pageNumber > 1,
                totalCount = totalCount,
                totalPages = (int)Math.Ceiling((double)totalCount / filter.pageSize),
                contracts = list
            };

            return c;
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

        public async Task<bool> UpdateContractData(EditContractDto updatedContract)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            string sqlAdmin = @"
            UPDATE Contracts
            SET 
                Position = @Position,
                Description = @Description,
                StartDate = @StartDate,
                EndDate = @EndDate,
                Wage = @Wage
            WHERE Id = @ContractId";
            string sqlUser = @"
            UPDATE Contracts
            SET 
                Position = @Position,
                Description = @Description
            WHERE Id = @ContractId";
            var sql = updatedContract.hasAdminAccess ? sqlAdmin : sqlUser;
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Position", updatedContract.contract.Position ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Description", updatedContract.contract.Description ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@StartDate", updatedContract.contract.StartDate);
            cmd.Parameters.AddWithValue("@EndDate", updatedContract.contract.EndDate);
            cmd.Parameters.AddWithValue("@Wage", updatedContract.contract.Wage);
            cmd.Parameters.AddWithValue("@ContractId", updatedContract.contract.Id);
            int rowsAffected = await cmd.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }

        public async Task<string> getUserAccessLevel(int contractId, int userId)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new SqlCommand(@"
                SELECT c.CompanyId, co.AdminId, c.UserId
                FROM Contracts c
                JOIN Companies co ON c.CompanyId = co.Id
                WHERE c.Id = @cid;
            ", conn);

            cmd.Parameters.AddWithValue("@cid", contractId);

            using var reader = await cmd.ExecuteReaderAsync();
            if (!reader.Read()) return "Denied";

            int companyAdminId = (int)reader["AdminId"];
            int companyUserId = (int)reader["UserId"];
            return userId == companyAdminId? "Full": (userId == companyUserId? "Partial":"Denied");
        }
    }
}
