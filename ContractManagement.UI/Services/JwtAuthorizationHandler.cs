using Blazored.LocalStorage;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
namespace ContractManagement.UI.Services
{
    using Microsoft.JSInterop;

    public class JwtAuthorizationHandler : DelegatingHandler
    {
        private readonly ILocalStorageService _localStorage;
        private readonly IJSRuntime _jsRuntime;
        private const string TokenKey = "jwt_token";

        public JwtAuthorizationHandler(ILocalStorageService localStorage, IJSRuntime jsRuntime)
        {
            _localStorage = localStorage;
            _jsRuntime = jsRuntime;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Check if JS interop is available
            if (_jsRuntime is IJSInProcessRuntime)
            {
                var token = await _localStorage.GetItemAsync<string>(TokenKey);
                if (!string.IsNullOrEmpty(token))
                {
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }

}