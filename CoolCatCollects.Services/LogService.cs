using CoolCatCollects.Data.Entities;
using CoolCatCollects.Data.Repositories;
using CoolCatCollects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoolCatCollects.Services
{
	public class LogService : IDisposable
	{
		public BaseRepository<Log> _repo = new BaseRepository<Log>();

		public async Task<IEnumerable<LogModel>> GetAll()
		{
			var logs = await _repo.FindAllAsync();

			return logs.Select(ToModel);
		}

		public async Task<LogModel> FindAsync(int id)
		{
			var log = await _repo.FindOneAsync(id);

			return ToModel(log);
		}

		public async Task Add(LogModel model)
		{
			var log = new Log
			{
				Date = DateTime.Now,
				Title = model.Title,
				Note = model.Note,
				FurtherNote = model.FurtherNote,
				Category = model.Category
			};

			await _repo.AddAsync(log);
		}

		public async Task Edit(LogModel model)
		{
			var log = await _repo.FindOneAsync(model.Id);

			log.Title = model.Title;
			log.Note = model.Note;
			log.FurtherNote = model.FurtherNote;
			log.Category = model.Category;

			await _repo.UpdateAsync(log);
		}

		public async Task Delete(int id)
		{
			var log = await _repo.FindOneAsync(id);

			await _repo.RemoveAsync(log);
		}

		public LogModel ToModel(Log log)
		{
			return new LogModel
			{
				Id = log.Id,
				Date = log.Date,
				Title = log.Title,
				Note = log.Note,
				FurtherNote = log.FurtherNote,
				Category = log.Category
			};
		}

		public void Dispose()
		{
			_repo.Dispose();
		}
	}
}
