using ContractManagement.BL.Interfaces;
using ContractManagement.Model.DTO;
using ContractManagement.BL.Entities;

namespace ContractManagement.BL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<UserDto>> GetAllUsers()
        {
            var response =  await _userRepository.GetAll();
            List<UserDto> users = new ();
            foreach (var user in response)
            {
                users.Add(new UserDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName
                });
            }
            return users;
        }

        public async Task<UserDto?> GetUserById(int id)
        {
            var response = await _userRepository.GetById(id);
            return new UserDto
            {
                Id = response.Id,
                FirstName = response.FirstName,
                LastName = response.LastName
            };
        }

        public async Task<UserDto?> GetUserByUsername(string username)
        {
            var response = await _userRepository.GetUserByUsername(username);
            if (response == null) return null;
            return new UserDto
            {
                Id = response.Id,
                FirstName = response.FirstName,
                LastName = response.LastName
            };
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
        public async Task<bool> UpdateUserAsync(UserDto user)
        {
            return await _userRepository.UpdateUserAsync(user);
        }


        // Example password hashing method
        //private string HashPassword(string password)
        //{
        //    // Simple example — use proper hashing in production
        //    return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
        //}
    }
}
