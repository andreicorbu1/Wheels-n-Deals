using System.Linq.Expressions;

namespace Wheels_n_Deals.API.DataLayer.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T?> AddAsync(T entity);
        Task<T?> DeleteAsync(Guid id);
        Task DeleteManyAsync(Expression<Func<T, bool>> filter);
        Task<List<T>> GetAllAsync();
        Task<T?> GetByIdAsync(Guid id);
        Task<List<T>> GetManyAsync(Expression<Func<T, bool>>? filter = null,
                                          Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
                                          int? top = null,
                                          int? skip = null);
        Task<T?> UpdateAsync(T entity);
        bool Any(Func<T, bool> predicate);
    }
}
