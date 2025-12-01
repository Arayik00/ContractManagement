using Blazored.LocalStorage;
using System.Threading.Tasks;
namespace ContractManagement.UI.Services
{
    public class JwtService
    {
        private readonly ILocalStorageService _localStorage;
        private const string TokenKey = "jwt_token";

        public JwtService(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public async Task SetTokenAsync(string token)
        {
            await _localStorage.SetItemAsync(TokenKey, token);
        }

        public async Task<string> GetTokenAsync()
        {
            return await _localStorage.GetItemAsync<string>(TokenKey);
        }

        public async Task<bool> IsLoggedInAsync()
        {
            var token = await GetTokenAsync();
            return !string.IsNullOrEmpty(token);
        }

        public async Task ClearTokenAsync()
        {
            await _localStorage.RemoveItemAsync(TokenKey);
        }
    }
}