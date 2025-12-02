using ContractManagement.Model.DTO;
using ContractManagement.Model.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractManagement.Database.Interfaces
{
    public interface IUserRepository
    {
        Task<Users>? GetById(int id);
        Task<List<Users>> GetAll();
        Task<Users?> GetByCredentialsAsync(string username, string password);

        Task<Users?> GetUserByUsername(string username);

        Task<bool> CreateUserAsync(Users user);
        Task<bool> UserHasCompanyAsync(int userId);

        Task<List<Contracts>> GetContractsByUserIdAsync(int userId);

        Task<bool> UpdateUserAsync(UserDto user);


    }
}
