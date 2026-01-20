using Microsoft.Build.Tasks.Deployment.Bootstrapper;
using Microsoft.EntityFrameworkCore.Storage;

using School.Application.Contracts;
using School.Infrastructure.Repositories;
using School.Models;
using School.Persistence;

using System;
using System.Collections.Concurrent;

namespace School.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SchoolContext _context;

        // Cache por tipo de entidad: typeof(T) -> repo instance
        private readonly ConcurrentDictionary<Type, object> _repositories = new();

        public UnitOfWork(SchoolContext context)
        {
            _context = context;
        }

        public IGenericRepository<T> Repository<T>() where T : class
        {
            // Si existe, devuelve el mismo repo. Si no, crea y cachea.
            var repo = _repositories.GetOrAdd(typeof(T), _ => new GenericRepository<T>(_context));
            return (IGenericRepository<T>)repo;
        }

        public Task<int> SaveChangesAsync(CancellationToken ct = default)
            => _context.SaveChangesAsync(ct);

        public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken ct = default)
       => _context.Database.BeginTransactionAsync(ct);

        public void Dispose()
            => _context.Dispose();
    }
}
