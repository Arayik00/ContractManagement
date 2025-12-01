using Blazored.LocalStorage;

namespace ContractManagement.Web.Services
{
    public class JwtService
    {
        private readonly ILocalStorageService _storage;

        public JwtService(ILocalStorageService storage)
        {
            _storage = storage;
        }

        public async Task SaveTokenAsync(string token)
        {
            await _storage.SetItemAsync("jwt", token);
        }

        public async Task<string?> GetTokenAsync()
        {
            return await _storage.GetItemAsync<string>("jwt");
        }

        public async Task ClearTokenAsync()
        {
            await _storage.RemoveItemAsync("jwt");
        }
    }
}