using System.Reflection.Metadata;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, CancellationToken) =>
    {
        document.Info.Title = "My First Web Api";
        document.Info.Version = "v1";
        document.Info.Description = "This is my first web API using ASP.NET Core.";
        return Task.CompletedTask;
    });
});
builder.Services.AddSingleton<MyFirstWebApi.Repositories.TodoItemRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "My First Web Api v1");
        options.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
        options.DocumentTitle = "My First Web Api Documentation";
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
