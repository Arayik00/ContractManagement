using Microsoft.JSInterop;
using System.Security.Claims;

namespace ContractManagement.UI.Services
{
    public class AuthService
    {
        private readonly IJSRuntime _js;
        private string? _jwt;

        public AuthService(IJSRuntime js)
        {
            _js = js;
        }

        public async Task<string?> GetTokenAsync()
        {
            if (_jwt is not null)
                return _jwt;

            _jwt = await _js.InvokeAsync<string>("localStorage.getItem", "jwt");
            return _jwt;
        }

        public async Task SetTokenAsync(string token)
        {
            _jwt = token;
            await _js.InvokeVoidAsync("localStorage.setItem", "jwt", token);
        }

        public async Task ClearTokenAsync()
        {
            _jwt = null;
            await _js.InvokeVoidAsync("localStorage.removeItem", "jwt");
        }

        public async Task<int?> GetAdminIdFromJwtAsync()
        {
            var jwt = await GetTokenAsync();
            if (string.IsNullOrEmpty(jwt))
                return null; // JWT not available, user not logged in
            var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(_jwt);
            var idClaim = token.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
                ?.Value;
            if (idClaim != null && int.TryParse(idClaim, out var id))
                return id;

            return null;
        }

         public async Task<bool> IsLoggedInAsync()
        {
            var token = await GetTokenAsync();
            return !string.IsNullOrEmpty(token);
        }
    }
}