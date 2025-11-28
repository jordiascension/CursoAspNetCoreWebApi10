var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

// Nuevo generador OpenAPI de ASP.NET Core
builder.Services.AddOpenApi(options =>
{
    // Opcional: personalizar el documento
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info.Title = "Mi API con Controladores";
        document.Info.Version = "v1";
        document.Info.Description = "Ejemplo usando AddOpenApi + Swagger UI en .NET 10";
        return Task.CompletedTask;
    });
});

builder.Services.AddSingleton<MyFirstWebApi.Repositories.TodoItemRepository>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    // Swagger UI (Swashbuckle) apuntando al documento generado por AddOpenApi
    app.UseSwaggerUI(options =>
    {
        // Ruta del JSON que expone MapOpenApi
        options.SwaggerEndpoint("/openapi/v1.json", "Mi API v1");
        options.RoutePrefix = "";   
        options.DocumentTitle = "Mi Swagger UI";
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
