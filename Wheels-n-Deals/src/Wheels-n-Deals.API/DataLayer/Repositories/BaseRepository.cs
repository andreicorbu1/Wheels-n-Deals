using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Wheels_n_Deals.API.DataLayer.Interfaces;
using Wheels_n_Deals.API.Infrastructure.CustomExceptions;

namespace Wheels_n_Deals.API.DataLayer.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public BaseRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        /// <summary>
        /// Adds an entity.
        /// </summary>
        /// <param name="entity">The entity to add</param>
        /// <returns>The entity that was added</returns>
        public async Task<T?> AddAsync(T entity)
        {
            if (_dbSet is null) return null;

            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public bool Any(Func<T, bool> predicate)
        {
            if (_dbSet is null) return false;

            return _dbSet.Any(predicate);
        }

        /// <summary>
        /// Deletes an entity.
        /// </summary>
        /// <param name="id">The id of the entity to delete</param>
        /// <returns><see cref="Task"/></returns>
        public async Task<T?> DeleteAsync(Guid id)
        {
            var entity = await GetByIdAsync(id);

            if (entity is null) return null;

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        /// <summary>
        /// Deletes entities based on a condition.
        /// </summary>
        /// <param name="filter">The condition the entities must fulfil to be deleted</param>
        /// <returns><see cref="Task"/></returns>
        public async Task DeleteManyAsync(Expression<Func<T, bool>> filter)
        {
            var entities = _dbSet.Where(filter);

            _dbSet.RemoveRange(entities);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Gets a collection of all entities.
        /// </summary>
        /// <returns>A collection of all entities</returns>
        public async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        /// <summary>
        /// Gets an entity by ID.
        /// </summary>
        /// <param name="id">The ID of the entity to retrieve</param>
        /// <returns>The entity object if found, otherwise null</returns>
        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        /// <summary>
        /// Gets a collection of entities based on the specified criteria.
        /// </summary>
        /// <param name="filter">The condition the entities must fulfil to be returned</param>
        /// <param name="orderBy">The function used to order the entities</param>
        /// <param name="top">The number of records to limit the results to</param>
        /// <param name="skip">The number of records to skip</param>
        /// <returns>A collection of entities</returns>
        public async Task<List<T>> GetManyAsync(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            int? top = null,
            int? skip = null)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (skip.HasValue)
            {
                query = query.Skip(skip.Value);
            }

            if (top.HasValue)
            {
                query = query.Take(top.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<T?> UpdateAsync(T entity)
        {
            if (_dbSet is null) return null;

            _context.ChangeTracker.Clear();
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();

            return entity;
        }
    }
}
