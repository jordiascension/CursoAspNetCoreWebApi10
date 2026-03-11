using Microsoft.EntityFrameworkCore.Storage;

namespace School.Application.Contracts
{
    public interface IUnitOfWork
    {
        // Repo genérico cacheado
        IGenericRepository<T> Repository<T>() where T : class;

        Task<int> SaveChangesAsync(CancellationToken ct = default);

        Task BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    }
}
