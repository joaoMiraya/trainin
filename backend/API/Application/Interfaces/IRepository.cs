using System.Linq.Expressions;

public interface IRepository<T> where T : class
{
    Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default);
    T Update(T entity);
    T Delete(T entity);
    Task DeleteByIdAsync(object id, CancellationToken cancellationToken = default);
    Task<T?> GetByIdAsync(object id, CancellationToken cancellationToken = default);
    Task<T?> GetByIdAsync(CancellationToken cancellationToken = default, params object[] ids);
    Task<List<T>> GetAllAsync(int skip = 0, int take = 25, CancellationToken cancellationToken = default);
    Task<List<T>> GetAsync(Expression<Func<T, bool>>? filter = null, 
                          Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
                          string includeProperties = "",
                          int skip = 0, 
                          int take = 25,
                          CancellationToken cancellationToken = default);
    Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>>? filter = null, 
                                   string includeProperties = "",
                                   CancellationToken cancellationToken = default);
    Task<int> CountAsync(Expression<Func<T, bool>>? filter = null, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default);
}