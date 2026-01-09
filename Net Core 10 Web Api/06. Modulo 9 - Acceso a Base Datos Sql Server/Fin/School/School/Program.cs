using HealthChecks.UI.Client;

using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

using School.Extensions;
using School.Persistence;

using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var sqlServerConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services
    .AddHealthChecks()
    .AddSqlServer(
        sqlServerConnectionString!,
        name: "sqlserver",
        timeout: TimeSpan.FromSeconds(5),
        failureStatus: HealthStatus.Unhealthy
    );

builder.Services
    .AddHealthChecksUI(setup =>
    {
        setup.AddHealthCheckEndpoint("API + SQL", "/healthz");
        setup.SetEvaluationTimeInSeconds(10);
        setup.SetMinimumSecondsBetweenFailureNotifications(60);
    })
    .AddInMemoryStorage();

var cs = sqlServerConnectionString;
builder.Services.AddDbContext<SchoolContext>(opt => opt.UseSqlServer(cs));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// ✅ Endpoint para UI y para Docker/K8s (formato correcto UI)
app.MapHealthChecks("/healthz", new HealthCheckOptions
{
    Predicate = _ => true,
    ResultStatusCodes =
    {
        [HealthStatus.Healthy] = StatusCodes.Status200OK,
        [HealthStatus.Degraded] = StatusCodes.Status200OK,
        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
    },
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapHealthChecksUI(options =>
{
    options.UIPath = "/health-ui";
    options.ApiPath = "/health-ui-api";
});

// Apply pending migrations on application startup
/*using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SchoolContext>();
    // This will create the database and tables if they do not exist.
    // If everything is already up to date, it does nothing.
    db.Database.Migrate(); 
}*/

// 👇 Migraciones + seed
await app.InitializeDatabaseAsync();



app.Run();
