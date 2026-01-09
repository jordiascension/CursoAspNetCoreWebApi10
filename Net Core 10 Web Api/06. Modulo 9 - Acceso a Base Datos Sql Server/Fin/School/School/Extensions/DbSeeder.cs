using School.Models;
using School.Persistence;

using System;

namespace School.Extensions
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(SchoolContext db)
        {
            // Evita duplicar datos
            if (db.Students.Any())
                return;

            var students = new List<Student>
            {
                new() { FirstName = "Juan", LastName = "Pérez", DateOfBirth = new DateTime(2003, 5, 1) },
                new() { FirstName = "Maria", LastName = "Gómez", DateOfBirth = new DateTime(2001, 8, 15) },
                new() { FirstName = "Pedro", LastName = "López", DateOfBirth = new DateTime(2002, 3, 10) }
            };

            db.Students.AddRange(students);
            await db.SaveChangesAsync();
        }
    }
}