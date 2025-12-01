using ContractManagement.Model.Models;

namespace ContractManagement.BL.Interfaces
{
    public interface IUserService
    {
        Task<List<Users>> GetAllUsers();
        Task<Users?> GetUserById(int id);

        Task<Users?> GetUserByUsername(string username);

        Task<bool> CreateUserAsync(Users user);

        Task<bool> UserHasCompanyAsync(int userId);

        Task<List<Contracts>> GetContractsByUserIdAsync(int userId);


    }
}
