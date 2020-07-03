using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using CoolCatCollects.Services;
using CoolCatCollects.Models;
using CoolCatCollects.Bricklink;

namespace CoolCatCollects.Controllers
{
	public class NewPurchasesController : Controller
	{
		private NewPurchaseService _service = new NewPurchaseService();
		private const string _bindAll = "Id,Date,SetNumber,SetName,Theme,Promotions,Price,UnitPrice,Quantity,Parts,TotalParts,PriceToPartOutRatio,Source,PaymentMethod,AveragePartOutValue,MyPartOutValue,ExpectedGrossProfit,ExpectedNetProfit,Status,SellingNotes,Notes,MinifigureValue";

		// GET: NewPurchases
		public async Task<ActionResult> Index()
		{
			var purchases = await _service.GetAll();

			return View(purchases);
		}

		// GET: NewPurchases/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: NewPurchases/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Create([Bind(Include = _bindAll)] NewPurchaseModel newPurchase)
		{
			if (ModelState.IsValid)
			{
				await _service.Add(newPurchase);

				return RedirectToAction("Index");
			}

			return View(newPurchase);
		}

		// GET: NewPurchases/Edit/5
		public async Task<ActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			NewPurchaseModel newPurchase = await _service.FindAsync(id.Value);

			if (newPurchase == null)
			{
				return HttpNotFound();
			}
			return View(newPurchase);
		}

		// POST: NewPurchases/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit([Bind(Include = _bindAll)] NewPurchaseModel newPurchase)
		{
			if (ModelState.IsValid)
			{
				await _service.Edit(newPurchase);

				return RedirectToAction("Index");
			}
			return View(newPurchase);
		}

		// GET: NewPurchases/Delete/5
		public async Task<ActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			NewPurchaseModel newPurchase = await _service.FindAsync(id.Value);

			if (newPurchase == null)
			{
				return HttpNotFound();
			}
			return View(newPurchase);
		}

		// POST: NewPurchases/Delete/5
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

		public ActionResult GetSetInfo(string set)
		{
			var service = new BricklinkService();

			return Json(service.GetSetDetails(set), JsonRequestBehavior.AllowGet);
		}
	}
}
