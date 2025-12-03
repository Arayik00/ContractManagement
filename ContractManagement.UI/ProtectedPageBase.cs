using ContractManagement.Model.Entities;
using ContractManagement.UI.Services;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using YamlDotNet.Core.Tokens;
using static ContractManagement.UI.Components.Pages.Home;

namespace ContractManagement.UI
{
    public class ProtectedPageBase : ComponentBase
    {
        [Inject] protected AuthService authService { get; set; }
        [Inject] protected NavigationManager Nav { get; set; }

        public bool _checkedAuth = false;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!_checkedAuth && firstRender)
            {
                _checkedAuth = true;

                if (!await authService.IsLoggedInAsync())
                {
                    Nav.NavigateTo("/login", forceLoad: true);
                }
            }
        }
    }
}