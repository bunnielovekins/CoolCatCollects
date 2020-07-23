using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using CoolCatCollects.Services;
using CoolCatCollects.Models.Expenses;

namespace CoolCatCollects.Controllers
{
	public class ExpensesController : Controller
	{
		private ExpensesService _service = new ExpensesService();
		private const string _bindAll = "Id,Date,TaxCategory,Category,Amount,Source,Item,Quantity,ExpenditureType,OrderNumber,Price,Postage,Receipt,Notes";

		// GET: Expenses
		public async Task<ActionResult> Index()
		{
			var expenses = await _service.GetAll();

			return View(expenses);
		}

		// GET: Expenses/Details/5
		public async Task<ActionResult> Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			ExpenseModel expense = await _service.FindAsync(id.Value);

			if (expense == null)
			{
				return HttpNotFound();
			}
			return View(expense);
		}

		// GET: Expenses/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: Expenses/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Create([Bind(Include = _bindAll)] ExpenseModel expense)
		{
			if (ModelState.IsValid)
			{
				await _service.Add(expense);

				return RedirectToAction("Index");
			}

			return View(expense);
		}

		// GET: Expenses/Edit/5
		public async Task<ActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			ExpenseModel expense = await _service.FindAsync(id.Value);

			if (expense == null)
			{
				return HttpNotFound();
			}
			return View(expense);
		}

		// POST: Expenses/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit([Bind(Include = _bindAll)] ExpenseModel expense)
		{
			if (ModelState.IsValid)
			{
				await _service.Edit(expense);

				return RedirectToAction("Index");
			}
			return View(expense);
		}

		// GET: Expenses/Delete/5
		public async Task<ActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			ExpenseModel expense = await _service.FindAsync(id.Value);

			if (expense == null)
			{
				return HttpNotFound();
			}
			return View(expense);
		}

		// POST: Expenses/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> DeleteConfirmed(int id)
		{
			await _service.Delete(id);

			return RedirectToAction("Index");
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_service.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}
