using ContractManagement.BL.Interfaces;
using ContractManagement.BL.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractManagement.Database.Repositories
{
    public class CompanyRepository: ICompanyRepository
    {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;

        public CompanyRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<Companies>? GetById(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("SELECT * FROM Companies WHERE Id=@id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            await conn.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();
            if (!await reader.ReadAsync()) return null;

            return new Companies
            {
                Id = (int)reader["Id"],
                AdminId = (int)reader["AdminId"],
                CompanyName = (string)reader["CompanyName"],
                ShortDescription = (string)reader["ShortDescription"]
            };
        }
        public async Task<List<Companies>> GetAll()
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("SELECT * FROM Companies", conn);
            List<Companies> contracts = new List<Companies>();
            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                contracts.Add(new Companies
                {
                    Id = (int)reader["Id"],
                    AdminId = (int)reader["AdminId"],
                    CompanyName = (string)reader["CompanyName"],
                    ShortDescription = (string)reader["ShortDescription"]
                });
            }
            return contracts;
        }
        public async Task<List<Companies>> GetAllByAdminId(int adminId)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("SELECT * FROM Companies WHERE AdminId=@adminId", conn);
            cmd.Parameters.AddWithValue("@adminId", adminId);
            List<Companies> contracts = new List<Companies>();
            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                contracts.Add(new Companies
                {
                    Id = (int)reader["Id"],
                    AdminId = (int)reader["AdminId"],
                    CompanyName = (string)reader["CompanyName"],
                    ShortDescription = (string)reader["ShortDescription"]
                });
            }
            return contracts;
        }

        public async Task<int> InsertCompanyAsync(Companies company)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            // Insert company and get the new Id
            var cmd = new SqlCommand(@"
        INSERT INTO Companies (AdminId, CompanyName, ShortDescription)
        OUTPUT INSERTED.Id
        VALUES (@adminId, @companyName, @shortDescription)", conn);

            cmd.Parameters.AddWithValue("@adminId", company.AdminId);
            cmd.Parameters.AddWithValue("@companyName", company.CompanyName);
            cmd.Parameters.AddWithValue("@shortDescription", company.ShortDescription ?? (object)DBNull.Value);

            var newCompanyIdObj = await cmd.ExecuteScalarAsync();
            if (newCompanyIdObj == null) throw new Exception("Failed to insert company.");

            int newCompanyId = Convert.ToInt32(newCompanyIdObj);

            // Insert user role

            cmd = new SqlCommand(
                "INSERT INTO UserRoles (UserId, CompanyId, Role) VALUES (@adminId, @companyId, @role)",
                conn);

            cmd.Parameters.AddWithValue("@adminId", company.AdminId);
            cmd.Parameters.AddWithValue("@companyId", newCompanyId);
            cmd.Parameters.AddWithValue("@role", "admin");

            await cmd.ExecuteNonQueryAsync();

            return newCompanyId; // return the newly created company ID
        }
    }
}
