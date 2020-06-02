using CoolCatCollects.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CoolCatCollects.Data.Repositories
{
	public class BaseRepository<T> : IDisposable where T : BaseEntity
	{
		protected EfContext _ctx;

		public BaseRepository()
		{
			_ctx = new EfContext();
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

		public IEnumerable<T> FindAll()
		{
			return _ctx.Set<T>().ToList();
		}

		public IEnumerable<T> Find(Expression<Func<T, bool>> conditions)
		{
			return _ctx.Set<T>().Where(conditions);
		}

		public T FindOne(Expression<Func<T, bool>> conditions)
		{
			return _ctx.Set<T>().FirstOrDefault(conditions);
		}

		public virtual T Update(T entity)
		{
			var obj = _ctx.Set<T>().Find(entity.Id);

			if (obj == null)
			{
				return Add(entity);
			}

			_ctx.Entry(obj).CurrentValues.SetValues(entity);
			return obj;
		}
	}
}
