using DealershipAPI.Entity;
using Microsoft.EntityFrameworkCore;

namespace DealershipAPI.Repository;

public class BaseRepository<TEntity> where TEntity : BaseEntity
{
	protected readonly AppDbContext _context;
	private readonly DbSet<TEntity> _dbSet;

	public BaseRepository(AppDbContext context)
	{
		_context = context;
		_dbSet = _context.Set<TEntity>();
	}

	public TEntity GetById(Guid id)
	{
		return _dbSet.FirstOrDefault(entity => entity.Id == id);
	}

	public void Insert(TEntity entity)
	{
		_dbSet.Add(entity);
		_context.SaveChanges();
	}

	public void Delete(TEntity entity)
	{
		_dbSet.Remove(entity);
		_context.SaveChanges();
	}

	public void Update(TEntity entity)
	{
		_dbSet.Update(entity);
		_context.SaveChanges();
	}

	public List<TEntity> GetAll()
	{
		return GetRecords().ToList();
	}

	public bool Any(Func<TEntity, bool> expression)
	{
		return GetRecords().Any(expression);
	}

	protected IQueryable<TEntity> GetRecords()
	{
		return _dbSet.AsQueryable<TEntity>();
	}
}
