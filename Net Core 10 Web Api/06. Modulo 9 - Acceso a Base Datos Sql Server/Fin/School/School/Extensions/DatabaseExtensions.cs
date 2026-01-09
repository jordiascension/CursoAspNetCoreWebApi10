using Microsoft.EntityFrameworkCore;

using School.Persistence;

using System;

namespace School.Extensions
{
    public static class DatabaseExtensions
    {
        extension(WebApplication app)
        {
            public async Task InitializeDatabaseAsync()
            {
                using var scope = app.Services.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<SchoolContext>();

                // Aplica migraciones
                await db.Database.MigrateAsync();

                // Ejecuta seed
                await DbSeeder.SeedAsync(db);
            }
        }
    }
}
