using CryptoClient;
using CryptoClient.Contracts;
using CryptoClient.Services;

using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

using Radzen;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Register HttpClientFactory with a named client
builder.Services.AddHttpClient("CryptoApi", client =>
{
    client.BaseAddress = new Uri("https://localhost:7037/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

// Register our crypto service (fetch + SignalR)
builder.Services.AddSingleton<ICryptoService, CryptoService>();

// Radzen
builder.Services.AddRadzenComponents();
await builder.Build().RunAsync();
