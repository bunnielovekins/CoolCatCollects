using CoolCatCollects.Data.Entities.Expenses;
using CoolCatCollects.Data.Repositories;
using CoolCatCollects.Models.Expenses;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoolCatCollects.Services
{
	public class ExpensesService
	{
		public BaseRepository<Expense> _repo = new BaseRepository<Expense>();

		public async Task<IEnumerable<ExpenseModel>> GetAll()
		{
			var Expenses = await _repo.FindAllAsync();

			return Expenses.Select(ToModel).OrderByDescending(x => x.Date);
		}

		public async Task<ExpenseModel> FindAsync(int id)
		{
			var expense = await _repo.FindOneAsync(id);

			return ToModel(expense);
		}

		public async Task Add(ExpenseModel model)
		{
			var expense = new Expense
			{
				Id = model.Id,
				Date = model.Date,
				TaxCategory = model.TaxCategory,
				Category = model.Category,
				Source = model.Source,
				ExpenditureType = model.ExpenditureType,
				OrderNumber = model.OrderNumber,
				Price = model.Price,
				Postage = model.Postage,
				Receipt = model.Receipt,
				Notes = model.Notes,
				Item = model.Item,
				Quantity = model.Quantity
			};

			await _repo.AddAsync(expense);
		}

		public async Task Edit(ExpenseModel model)
		{
			var expense = await _repo.FindOneAsync(model.Id);

			expense.Id = model.Id;
			expense.Date = model.Date;
			expense.TaxCategory = model.TaxCategory;
			expense.Category = model.Category;
			expense.Source = model.Source;
			expense.ExpenditureType = model.ExpenditureType;
			expense.OrderNumber = model.OrderNumber;
			expense.Price = model.Price;
			expense.Postage = model.Postage;
			expense.Receipt = model.Receipt;
			expense.Notes = model.Notes;
			expense.Item = model.Item;
			expense.Quantity = model.Quantity;

			await _repo.UpdateAsync(expense);
		}

		public async Task Delete(int id)
		{
			var expense = await _repo.FindOneAsync(id);

			await _repo.RemoveAsync(expense);
		}

		public ExpenseModel ToModel(Expense expense)
		{
			return new ExpenseModel
			{
				Id = expense.Id,
				Date = expense.Date,
				TaxCategory = expense.TaxCategory,
				Category = expense.Category,
				Source = expense.Source,
				ExpenditureType = expense.ExpenditureType,
				OrderNumber = expense.OrderNumber,
				Price = expense.Price,
				Postage = expense.Postage,
				Receipt = expense.Receipt,
				Notes = expense.Notes,
				Item = expense.Item,
				Quantity = expense.Quantity
			};
		}

		public void Dispose()
		{
			_repo.Dispose();
		}
	}
}
