using HealthChecks.UI.Client;

using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

using School.Application.Contracts;
using School.Application.Implementations;
using School.Domain.Contracts;
using School.Domain.Implementations;
using School.Extensions;
using School.Infrastructure.UnitOfWork;
using School.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
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

builder.Services.AddScoped<IInvoicingAppService, InvoicingAppService>();
builder.Services.AddScoped<IInvoicingDomainService, InvoicingDomainService>();
builder.Services.AddDbContext<SchoolContext>(opt => opt.UseSqlServer(sqlServerConnectionString));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var app = builder.Build();

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

// 👇 Migraciones + seed
await app.InitializeDatabaseAsync();

app.Run();
