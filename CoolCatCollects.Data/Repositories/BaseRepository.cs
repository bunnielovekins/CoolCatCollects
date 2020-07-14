using CoolCatCollects.Data.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CoolCatCollects.Data.Repositories
{
	public class BaseRepository<T> : IDisposable where T : BaseEntity
	{
		protected EfContext _ctx;

		public EfContext Context { get => _ctx; }

		public BaseRepository()
		{
			_ctx = new EfContext();
		}



		public BaseRepository(EfContext context)
		{
			_ctx = context;
		}

		public void Dispose()
		{
			_ctx.Dispose();
		}

		public T Add(T entity)
		{
			var obj = _ctx.Set<T>().Add(entity);
			_ctx.SaveChanges();

			return obj;
		}

		public async Task AddAsync(T entity)
		{
			_ctx.Set<T>().Add(entity);

			await _ctx.SaveChangesAsync();
		}

		public IEnumerable<T> FindAll()
		{
			return _ctx.Set<T>().ToList();
		}

		public async Task<IEnumerable<T>> FindAllAsync()
		{
			return await _ctx.Set<T>().ToListAsync();
		}

		public IEnumerable<T> Find(Expression<Func<T, bool>> conditions)
		{
			return _ctx.Set<T>().Where(conditions);
		}

		public T FindOne(Expression<Func<T, bool>> conditions)
		{
			return _ctx.Set<T>().FirstOrDefault(conditions);
		}

		public T FindOne(int id)
		{
			return _ctx.Set<T>().Find(id);
		}

		public async Task<T> FindOneAsync(int id)
		{
			return await _ctx.Set<T>().FindAsync(id);
		}

		public virtual T Update(T entity)
		{
			var obj = _ctx.Set<T>().Find(entity.Id);

			if (obj == null)
			{
				return Add(entity);
			}

			_ctx.Entry(obj).CurrentValues.SetValues(entity);

			_ctx.SaveChanges();
			return obj;
		}

		public async Task UpdateAsync(T entity)
		{
			var obj = await _ctx.Set<T>().FindAsync(entity.Id);

			if (obj == null)
			{
				await AddAsync(entity);
				return;
			}

			_ctx.Entry(obj).CurrentValues.SetValues(entity);

			await _ctx.SaveChangesAsync();
		}

		public void Remove(T entity)
		{
			_ctx.Set<T>().Remove(entity);

			_ctx.SaveChanges();
		}

		public async Task RemoveAsync(T entity)
		{
			_ctx.Set<T>().Remove(entity);

			await _ctx.SaveChangesAsync();
		}

		public void RemoveMany(IEnumerable<T> entities)
		{
			_ctx.Set<T>().RemoveRange(entities);

			_ctx.SaveChanges();
		}

		public async Task RemoveManyAsync(IEnumerable<T> entities)
		{
			_ctx.Set<T>().RemoveRange(entities);

			await _ctx.SaveChangesAsync();
		}
	}
}
