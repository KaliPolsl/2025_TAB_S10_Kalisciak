using AquaparkApp.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using AquaparkApp.Client.Services;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


builder.Services.AddHttpClient("AquaparkApp.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

// Supply HttpClient instances that include access tokens when making requests to the server project
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("AquaparkApp.ServerAPI"));


builder.Services.AddScoped<BrowserWindowSizeProvider>();


builder.Services.AddScoped<AtrakcjaService>();

// Inicjalizacja serwisu po uruchomieniu aplikacji
builder.Services.AddScoped(async sp =>
{
    var service = sp.GetRequiredService<BrowserWindowSizeProvider>();
    await service.Initialize();
    return service;
});


await builder.Build().RunAsync();
