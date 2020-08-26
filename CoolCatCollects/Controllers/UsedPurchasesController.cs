using CoolCatCollects.Models;
using CoolCatCollects.Services;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

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

			UsedPurchaseModel usedPurchase = await _service.Find(id.Value);

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

			UsedPurchaseModel usedPurchase = await _service.Find(id.Value);

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

		// GET: UsedPurchases/Contents/5
		public async Task<ActionResult> Contents(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			var entity = await _service.GetPurchaseWithContents(id.Value);

			if (entity == null)
			{
				return HttpNotFound();
			}

			var model = new UsedPurchaseWeightsViewModel
			{
				Purchase = entity,
				Weights = entity.Weights,
				BLUploads = entity.BLUploads,
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
					"Pink",
					"Classic"
				}.Select(x => new SelectListItem { Text = x, Value = x })
			};

			return View(model);
		}

		[HttpPost]
		public async Task<ActionResult> Contents(UsedPurchaseWeightsViewModel model)
		{
			await _service.UpdateWeights(model.Purchase.Id, model.Weights);

			return RedirectToAction("Index");
		}

		public async Task<ActionResult> AddBLUpload(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			UsedPurchaseModel usedPurchase = await _service.Find(id.Value);

			if (usedPurchase == null)
			{
				return HttpNotFound();
			}

			TempData["id"] = usedPurchase.Id;

			return View();
		}

		[HttpPost]
		public async Task<ActionResult> AddBLUpload(UsedPurchaseBLUploadModel model)
		{
			if (ModelState.IsValid)
			{
				await _service.AddBLUpload(model);

				return RedirectToAction("Contents", new { id = model.UsedPurchaseId });
			}

			return View(model);
		}

		public async Task<ActionResult> EditBLUpload(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			UsedPurchaseBLUploadModel model = await _service.FindBLUpload(id.Value);

			if (model == null)
			{
				return HttpNotFound();
			}

			return View(model);
		}

		[HttpPost]
		public async Task<ActionResult> EditBLUpload(UsedPurchaseBLUploadModel model)
		{
			if (ModelState.IsValid)
			{
				await _service.EditBLUpload(model);

				return RedirectToAction("Contents", new { id = model.UsedPurchaseId });
			}
			return View(model);
		}

		public async Task<ActionResult> DeleteBLUpload(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			UsedPurchaseBLUploadModel model = await _service.FindBLUpload(id.Value);

			if (model == null)
			{
				return HttpNotFound();
			}
			return View(model);
		}

		[HttpPost, ActionName("DeleteBLUpload")]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> DeleteConfirmedBLUpload(int id)
		{
			await _service.DeleteBLUpload(id);

			return RedirectToAction("Index");
		}

		public class UsedPurchaseWeightsViewModel
		{
			public UsedPurchaseModel Purchase { get; set; }
			public IEnumerable<UsedPurchaseWeightModel> Weights { get; set; }
			public IEnumerable<UsedPurchaseBLUploadModel> BLUploads { get; set; }
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
