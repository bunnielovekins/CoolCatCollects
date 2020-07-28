using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using CoolCatCollects.Services;
using CoolCatCollects.Models;
using CoolCatCollects.Data.Entities.Purchases;
using System.Collections.Generic;
using CoolCatCollects.Bricklink.Models;
using CoolCatCollects.Bricklink;
using System.Linq;

namespace CoolCatCollects.Controllers
{
	public class UsedPurchasesController : Controller
	{
		private UsedPurchaseService _service = new UsedPurchaseService();
		private const string _bindAll = "Id,Date,Source,SourceUsername,OrderNumber,Price,PaymentMethod,Receipt,DistanceTravelled,Location,Postage,Weight,PricePerKilo,CompleteSets,Notes";

		// GET: UsedPurchases
		public async Task<ActionResult> Index()
		{
			var purchases = await _service.GetAll();

			return View(purchases);
		}

		// GET: UsedPurchases/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: UsedPurchases/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Create([Bind(Include = _bindAll)] UsedPurchaseModel usedPurchase)
		{
			if (ModelState.IsValid)
			{
				await _service.Add(usedPurchase);

				return RedirectToAction("Index");
			}

			return View(usedPurchase);
		}

		// GET: UsedPurchases/Edit/5
		public async Task<ActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			UsedPurchaseModel usedPurchase = await _service.FindAsync(id.Value);

			if (usedPurchase == null)
			{
				return HttpNotFound();
			}
			return View(usedPurchase);
		}

		// POST: UsedPurchases/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit([Bind(Include = _bindAll)] UsedPurchaseModel usedPurchase)
		{
			if (ModelState.IsValid)
			{
				await _service.Edit(usedPurchase);

				return RedirectToAction("Index");
			}
			return View(usedPurchase);
		}

		// GET: UsedPurchases/Delete/5
		public async Task<ActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			UsedPurchaseModel usedPurchase = await _service.FindAsync(id.Value);

			if (usedPurchase == null)
			{
				return HttpNotFound();
			}
			return View(usedPurchase);
		}

		// POST: UsedPurchases/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> DeleteConfirmed(int id)
		{
			await _service.Delete(id);

			return RedirectToAction("Index");
		}

		// GET: UsedPurchases/Weights/5
		public async Task<ActionResult> Weights(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			var entity = await _service.GetPurchaseWithWeights(id.Value);

			if (entity == null)
			{
				return HttpNotFound();
			}

			var model = new UsedPurchaseWeightsViewModel
			{
				Purchase = entity,
				Weights = entity.Weights,
				Colours = new List<string> {
					"Grey",
					"White",
					"Black",
					"Technic",
					"Red",
					"Yellow",
					"Blue",
					"Tan",
					"Brown",
					"Green",
					"Orange",
					"Dark Red",
					"Pink"
				}.Select(x => new SelectListItem { Text = x, Value = x })
			};

			return View(model);
		}

		[HttpPost]
		public async Task<ActionResult> Weights(UsedPurchaseWeightsViewModel model)
		{
			await _service.UpdateWeights(model.Purchase.Id, model.Weights);

			return RedirectToAction("Index");
		}

		public class UsedPurchaseWeightsViewModel
		{
			public UsedPurchaseModel Purchase { get; set; }
			public IEnumerable<UsedPurchaseWeightModel> Weights { get; set; }
			public IEnumerable<SelectListItem> Colours { get; set; }
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
