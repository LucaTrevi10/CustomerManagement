using CustomerManagementApp;
using CustomerManagementApp.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazorise;
using Blazorise.Bootstrap;
using CustomerManagementApp.Services.Mock;
using Blazorise.Icons.FontAwesome;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<LocalStorageService>();
builder.Services.AddScoped<AuthService>();

// Configurazione di Blazorise
builder.Services
    .AddBlazorise(options =>
    {
        options.Immediate = true; // Opzione per rendere i cambiamenti immediati
    })
    .AddBootstrapProviders()  // Usa Bootstrap come framework CSS
    .AddFontAwesomeIcons();   // Per le icone di FontAwesome   

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5202") });

bool useMock = false; // Configurazione per scegliere il servizio di mock
if (useMock)
{
    //builder.Services.AddScoped<ICustomerService, MockCustomerService>();
}
else
{
   builder.Services.AddScoped<ICustomerService, CustomerService>();
}

// Registrazione del servizio ProjectService
builder.Services.AddSingleton<IProjectService, ProjectService>();

// Registrare AutoMapper e i suoi profili
//builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);



await builder.Build().RunAsync();
