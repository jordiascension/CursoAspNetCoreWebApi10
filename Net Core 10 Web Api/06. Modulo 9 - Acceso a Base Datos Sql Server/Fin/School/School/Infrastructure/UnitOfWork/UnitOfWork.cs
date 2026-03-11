using Microsoft.EntityFrameworkCore.Storage;

using School.Application.Contracts;
using School.Infrastructure.Repositories;
using School.Persistence;

using System.Collections.Concurrent;

namespace School.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IAsyncDisposable, IDisposable
    {
        private readonly SchoolContext _context;

        private IDbContextTransaction? _currentTx;

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

        

        public async Task CommitTransactionAsync(CancellationToken ct = default)
        {
            if (_currentTx is null) return;

            await _currentTx.CommitAsync(ct);
            await _currentTx.DisposeAsync();
            _currentTx = null;
        }

        public async Task RollbackTransactionAsync(CancellationToken ct = default)
        {
            if (_currentTx is null) return;

            await _currentTx.RollbackAsync(ct);
            await _currentTx.DisposeAsync();
            _currentTx = null;
        }

        public void Dispose()
            => _context.Dispose();

        public ValueTask DisposeAsync() => _context.DisposeAsync();

        public async Task BeginTransactionAsync(CancellationToken ct = default)
        {
            if (_currentTx is not null) return; // ya hay una abierta
            _currentTx = await _context.Database.BeginTransactionAsync(ct);
        }
    }
}
