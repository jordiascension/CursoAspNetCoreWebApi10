using CriptoApi.Api.Hubs;
using CriptoApi.Api.Services;
using CriptoApi.Application;
using CriptoApi.Domain;
using CriptoApi.Infrastructure;

// Entry point of the API.
// Configures DI, CORS, controllers, SignalR, and background services.
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// ----- DOMAIN + INFRASTRUCTURE -----
builder.Services.AddSingleton<ICryptoRepository, InMemoryCryptoRepository>();

// ----- APPLICATION -----
builder.Services.AddSingleton<ICryptoPriceService, CryptoPriceService>();

// ----- SIGNALR + BACKGROUND -----
builder.Services.AddSignalR();
builder.Services.AddHostedService<PriceBroadcastService>();

builder.Services.AddControllers();

// ----- CORS -----
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyFrontend", policy =>
    {
        policy
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            // Aceptar cualquier origen (incluido 'null' de file://)
            .SetIsOriginAllowed(_ => true);
    });
});


// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

//Add configured Cors
app.UseCors("AllowAnyFrontend"); 

app.UseAuthorization();

app.MapControllers();

//Add SignalR Hub
app.MapHub<CryptoHub>("/hubs/crypto");

app.Run();
