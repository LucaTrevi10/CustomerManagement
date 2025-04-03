using Blazored.LocalStorage;
using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using CustomerUI;
using CustomerUI.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Text.Json;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddBlazoredLocalStorage();

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
builder.Services.AddScoped<SalesPipelineService>();

builder.Services.AddMudServices();


builder.Services.AddHttpClient("BackendAPI", client =>
{
    client.BaseAddress = new Uri("http://localhost:5202/");
}).AddHttpMessageHandler(sp => new AuthorizationMessageHandler(sp.GetRequiredService<ILocalStorageService>()));

builder.Services.AddHttpClient("CustomerService", client =>
{
    client.BaseAddress = new Uri("http://localhost:5089/");
}).AddHttpMessageHandler(sp => new AuthorizationMessageHandler(sp.GetRequiredService<ILocalStorageService>()));

builder.Services.AddHttpClient("PaymentService", client =>
{
    client.BaseAddress = new Uri("http://localhost:5125/");
}).AddHttpMessageHandler(sp => new AuthorizationMessageHandler(sp.GetRequiredService<ILocalStorageService>()));

builder.Services.AddHttpClient("ProjectService", client =>
{
    client.BaseAddress = new Uri("http://localhost:5177/");
}).AddHttpMessageHandler(sp => new AuthorizationMessageHandler(sp.GetRequiredService<ILocalStorageService>()));

builder.Services.AddHttpClient("SaleService", client =>
{
    client.BaseAddress = new Uri("http://localhost:5092");
}).AddHttpMessageHandler(sp => new AuthorizationMessageHandler(sp.GetRequiredService<ILocalStorageService>()));


static async void AddAuthorizationHeader(HttpClient client, ILocalStorageService localStorage)
{
    var token = await localStorage.GetItemAsStringAsync("token");
    if (!string.IsNullOrEmpty(token))
    {
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
    }
}

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