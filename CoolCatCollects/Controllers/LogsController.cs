using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using CoolCatCollects.Services;
using CoolCatCollects.Models;
using System;

namespace CoolCatCollects.Controllers
{
	public class LogsController : Controller
	{
		private LogService _logService = new LogService();

		// GET: Logs
		public async Task<ActionResult> Index()
		{
			return View(await _logService.GetAll());
		}

		// GET: Logs/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: Logs/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Create([Bind(Include = "Id,Date,Title,Note,Category")] LogModel log)
		{
			log.Date = DateTime.Now;

			if (ModelState.IsValid)
			{
				await _logService.Add(log);

				return RedirectToAction("Index");
			}

			return View(log);
		}

		// GET: Logs/Edit/5
		public async Task<ActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			LogModel log = await _logService.FindAsync(id.Value);

			if (log == null)
			{
				return HttpNotFound();
			}
			return View(log);
		}

		// POST: Logs/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit([Bind(Include = "Id,Date,Title,Note,Category")] LogModel log)
		{
			if (ModelState.IsValid)
			{
				await _logService.Edit(log);

				return RedirectToAction("Index");
			}
			return View(log);
		}

		// GET: Logs/Delete/5
		public async Task<ActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			LogModel log = await _logService.FindAsync(id.Value);

			if (log == null)
			{
				return HttpNotFound();
			}
			return View(log);
		}

		// POST: Logs/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> DeleteConfirmed(int id)
		{
			await _logService.Delete(id);

			return RedirectToAction("Index");
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_logService.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}
