using ContractManagement.Model.DTO;
using ContractManagement.BL.Entities;


namespace ContractManagement.BL.Interfaces
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAllUsers();
        Task<UserDto?> GetUserById(int id);

        Task<UserDto?> GetUserByUsername(string username);

        Task<bool> CreateUserAsync(Users user);

        Task<bool> UserHasCompanyAsync(int userId);

        Task<List<Contracts>> GetContractsByUserIdAsync(int userId);

        Task<bool> UpdateUserAsync(UserDto user);


    }
}
