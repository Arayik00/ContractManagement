using ContractManagement.BL.Interfaces;
using ContractManagement.Database.Interfaces;
using ContractManagement.Model.Models;
namespace ContractManagement.BL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<Users>> GetAllUsers()
        {
            return await _userRepository.GetAll();
        }

        public async Task<Users?> GetUserById(int id)
        {
            return await _userRepository.GetById(id);
        }

        public async Task<Users?> GetUserByUsername(string username)
        {
            return await _userRepository.GetUserByUsername(username);
        }

        public async Task<bool> CreateUserAsync(Users user)
        {
            return await _userRepository.CreateUserAsync(user);
        }

        public async Task<bool> UserHasCompanyAsync(int userId)
        {
            return await _userRepository.UserHasCompanyAsync(userId);
        }

        public async Task<List<Contracts>> GetContractsByUserIdAsync(int userId)
        {
            return await _userRepository.GetContractsByUserIdAsync(userId);
        }


        // Example password hashing method
        //private string HashPassword(string password)
        //{
        //    // Simple example — use proper hashing in production
        //    return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
        //}
    }
}
