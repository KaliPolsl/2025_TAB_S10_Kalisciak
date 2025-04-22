using AquaparkApp.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using AquaparkApp.Client.Services;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


builder.Services.AddHttpClient("AquaparkApp.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

// Supply HttpClient instances that include access tokens when making requests to the server project
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("AquaparkApp.ServerAPI"));


builder.Services.AddScoped<BrowserWindowSizeProvider>();


builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddAuthorizationCore();

// Inicjalizacja serwisu po uruchomieniu aplikacji
builder.Services.AddScoped(async sp =>
{
    var service = sp.GetRequiredService<BrowserWindowSizeProvider>();
    await service.Initialize();
    return service;
});


await builder.Build().RunAsync();
