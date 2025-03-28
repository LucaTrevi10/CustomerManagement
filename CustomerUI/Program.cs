using Blazored.LocalStorage;
using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using CustomerUI;
using CustomerUI.Auth;
using CustomerUI.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5202/api/") });

builder.Services
    .AddBlazorise(options =>
    {
        options.Immediate = true;
    })
    .AddBootstrapProviders()
    .AddFontAwesomeIcons();

builder.Services.AddScoped<JourneyService>();
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<ProjectService>();
builder.Services.AddScoped<PaymentService>();
builder.Services.AddScoped<AuthService>();

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddAuthorizationCore();

builder.Services.AddMudServices();

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddHttpClient("LoginService", client =>
{
    client.BaseAddress = new Uri("http://localhost:5090/");
});

builder.Services.AddHttpClient("BackendAPI", client =>
{
    client.BaseAddress = new Uri("http://localhost:5202/");
});

builder.Services.AddHttpClient("CustomerService", client =>
{
    client.BaseAddress = new Uri("http://localhost:5089/");
});

builder.Services.AddHttpClient("PaymentService", client =>
{
    client.BaseAddress = new Uri("http://localhost:5125/");
});

builder.Services.AddHttpClient("ProjectService", client =>
{
    client.BaseAddress = new Uri("http://localhost:5177");
});


//// Configura il JsonSerializerOptions
//builder.Services.AddScoped(sp => new HttpClient
//{
//    BaseAddress = new Uri("http://localhost:5202"),
//    DefaultRequestHeaders = { { "Accept", "application/json" } }
//});

builder.Services.AddScoped(_ =>
{
    var options = new JsonSerializerOptions
    {
        ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve,
        PropertyNameCaseInsensitive = true
    };

    return options;
});


await builder.Build().RunAsync();
