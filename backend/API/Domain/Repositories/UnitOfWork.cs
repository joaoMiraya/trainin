
// Implementação do Unit of Work
using API.Application.Interfaces;
using API.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

public class UnitOfWork : IUnitOfWork
{
    private readonly DbContext _context;
    private IDbContextTransaction? _transaction;
    private bool _disposed = false;
    private INotificationContext _notificationContext;

    public UnitOfWork(DbContext context, INotificationContext notificationContext)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _notificationContext = notificationContext;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
             _notificationContext.AddError("An error occurred while saving async changes: ",
                ex.Message, NotificationSystemType.Error.ToString());
            throw;
        }
    }

    public int SaveChanges()
    {
        try
        {
            return _context.SaveChanges();
        }
        catch (Exception ex)
        {
            _notificationContext.AddError("An error occurred while saving changes: ",
                ex.Message, NotificationSystemType.Error.ToString());
            throw;
        }
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            throw new InvalidOperationException("Transaction is already started");            
        }

        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction == null)
        {
            throw new InvalidOperationException("No transaction to commit");
        }

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            await _transaction.CommitAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            await RollbackTransactionAsync(cancellationToken);

            _notificationContext.AddError("An error occurred while committing transaction: ",
                ex.Message, NotificationSystemType.Error.ToString());
            throw;
        }
        finally
        {
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction == null)
        {
            _notificationContext.AddInfo("No transaction to rollback",
                NotificationSystemType.Info.ToString());
            return;
        }

        try
            {
                await _transaction.RollbackAsync(cancellationToken);
            }
            finally
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
    }

    public void ClearChangeTracker()
    {
        _context.ChangeTracker.Clear();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}