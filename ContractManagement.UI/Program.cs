using Blazored.LocalStorage;
using ContractManagement.UI.Components;
using ContractManagement.UI.Services;
using MudBlazor.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddRazorComponents()
       .AddInteractiveServerComponents(); // no prerender

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddMudServices();
// JWT & Auth
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddTransient<JwtAuthorizationHandler>();

// HttpClient with BaseAddress
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7519/")
});


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.Run();
