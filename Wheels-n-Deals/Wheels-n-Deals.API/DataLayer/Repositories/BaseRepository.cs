using Microsoft.EntityFrameworkCore;
using Wheels_n_Deals.API.DataLayer.Entities;

namespace Wheels_n_Deals.API.DataLayer.Repositories;

public class BaseRepository<T> where T : BaseEntity
{
    protected readonly AppDbContext _appDbContext;
    private readonly DbSet<T> _dbSet;

    public BaseRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
        _dbSet = appDbContext.Set<T>();
    }

    protected IQueryable<T> GetRecords()
    {
        return _dbSet.AsQueryable<T>();
    }

    public async Task<List<T>> GetAll()
    {
        return await GetRecords().ToListAsync();
    }

    public async Task<T?> GetById(Guid id)
    {
        if (_dbSet == null) return null;

        return await _dbSet.FindAsync(id);
    }

    public async Task<Guid?> Insert(T entity)
    {
        if (_dbSet == null) return null;
        if (entity.Id == Guid.Empty)
        {
            var idExists = await _dbSet.AnyAsync(e => e.Id == entity.Id);
            while (idExists)
            {
                entity.Id = Guid.NewGuid();
                idExists = await _dbSet.AnyAsync(e => e.Id == entity.Id);
            }
        }

        await _dbSet.AddAsync(entity);
        await _appDbContext.SaveChangesAsync();

        return entity.Id;
    }

    public async Task<T?> Remove(Guid id)
    {
        if (_dbSet == null) return null;

        var entity = await _dbSet.FindAsync(id);
        if (entity == null) return null;

        _dbSet.Remove(entity);
        await _appDbContext.SaveChangesAsync();

        return entity;
    }

    public async Task<T> Update(T entity)
    {
        _dbSet.Update(entity);
        await _appDbContext.SaveChangesAsync();
        return entity;
    }

    public bool Any(Func<T, bool> expression)
    {
        return GetRecords().Any(expression);
    }
}