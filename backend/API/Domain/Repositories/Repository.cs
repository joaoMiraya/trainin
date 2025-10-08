using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

// @todo implement notifications for validation errors
 
public abstract class Repository<T> : IRepository<T> where T : class
{
    protected readonly DbContext _context;
    protected readonly DbSet<T> _dbSet;

    protected Repository(DbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _dbSet = context.Set<T>();
    }

    public virtual async Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        await _dbSet.AddAsync(entity, cancellationToken);
        return entity;
    }

    public virtual T Update(T entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        _dbSet.Update(entity);
        return entity;
    }

    public virtual T Delete(T entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        _dbSet.Remove(entity);
        return entity;
    }

    public virtual async Task DeleteByIdAsync(object id, CancellationToken cancellationToken = default)
    {
        if (id == null)
            throw new ArgumentNullException(nameof(id));

        var entity = await _dbSet.FindAsync(new[] { id }, cancellationToken);
        if (entity != null)
        {
            _dbSet.Remove(entity);
        }
    }

    public virtual async Task<T?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
    {
        if (id == null)
            throw new ArgumentNullException(nameof(id));

        return await _dbSet.FindAsync(new[] { id }, cancellationToken);
    }

    public virtual async Task<T?> GetByIdAsync(CancellationToken cancellationToken = default, params object[] ids)
    {
        if (ids == null || ids.Length == 0)
            throw new ArgumentException("At least one ID must be provided", nameof(ids));

        return await _dbSet.FindAsync(ids, cancellationToken);
    }

    public virtual async Task<List<T>> GetAllAsync(int skip = 0, int take = 25, CancellationToken cancellationToken = default)
    {
        if (skip < 0)
            throw new ArgumentException("Skip must be non-negative", nameof(skip));
        if (take <= 0)
            throw new ArgumentException("Take must be positive", nameof(take));

        return await _dbSet
            .AsNoTracking()
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<List<T>> GetAsync(
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        string includeProperties = "",
        int skip = 0,
        int take = 25,
        CancellationToken cancellationToken = default)
    {
        if (skip < 0)
            throw new ArgumentException("Skip must be non-negative", nameof(skip));
        if (take <= 0)
            throw new ArgumentException("Take must be positive", nameof(take));

        IQueryable<T> query = _dbSet.AsNoTracking();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        foreach (var includeProperty in includeProperties.Split(
            new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty.Trim());
        }

        if (orderBy != null)
        {
            query = orderBy(query);
        }

        return await query
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<T?> GetFirstOrDefaultAsync(
        Expression<Func<T, bool>>? filter = null,
        string includeProperties = "",
        CancellationToken cancellationToken = default)
    {
        IQueryable<T> query = _dbSet.AsNoTracking();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        foreach (var includeProperty in includeProperties.Split(
            new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty.Trim());
        }

        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<int> CountAsync(Expression<Func<T, bool>>? filter = null, CancellationToken cancellationToken = default)
    {
        IQueryable<T> query = _dbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        return await query.CountAsync(cancellationToken);
    }

    public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default)
    {
        if (filter == null)
            throw new ArgumentNullException(nameof(filter));

        return await _dbSet.AnyAsync(filter, cancellationToken);
    }
}