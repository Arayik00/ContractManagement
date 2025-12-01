using Microsoft.JSInterop;
using System.Net.Http.Headers;
using System.Text.Json;
namespace ContractManagement.Web.Services
{
    public class AuthService
    {
        private readonly IJSRuntime _js;
        private readonly HttpClient _http;

        private const string TokenKey = "jwt_token";

        public AuthService(IJSRuntime js, HttpClient http)
        {
            _js = js;
            _http = http;
        }

        public async Task SaveTokenAsync(string token)
        {
            await _js.InvokeVoidAsync("localStorage.setItem", TokenKey, token);
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<string?> GetTokenAsync()
        {
            var token = await _js.InvokeAsync<string>("localStorage.getItem", TokenKey);
            if (!string.IsNullOrEmpty(token))
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return token;
        }

        public async Task ClearTokenAsync()
        {
            await _js.InvokeVoidAsync("localStorage.removeItem", TokenKey);
            _http.DefaultRequestHeaders.Authorization = null;
        }

        public async Task<bool> IsAuthenticatedAsync()
        {
            var token = await GetTokenAsync();
            if (string.IsNullOrEmpty(token))
                return false;

            // Optional: check expiry in token (decode JWT if you want)
            return true;
        }
    }
}