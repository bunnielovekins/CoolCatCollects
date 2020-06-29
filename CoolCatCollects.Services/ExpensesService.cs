using CoolCatCollects.Data.Entities.Expenses;
using CoolCatCollects.Data.Repositories;
using CoolCatCollects.Models.Expenses;
using System.Collections;

namespace CoolCatCollects.Services
{
	public class ExpensesService
	{
		private BaseRepository<Expense> _repo;

		public ExpensesService()
		{
			_repo = new BaseRepository<Expense>();
		}

		//public IEnumerable<ExpenseModel> GetAll
	}
}
