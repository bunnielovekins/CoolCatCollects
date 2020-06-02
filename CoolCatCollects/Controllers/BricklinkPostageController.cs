using CoolCatCollects.Bricklink;
using CoolCatCollects.Bricklink.Models;
using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web.Mvc;

namespace CoolCatCollects.Controllers
{
	public class BricklinkPostageController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult List()
		{
			var service = new BricklinkService();

			var result = service.GetOrders("PACKED");

			return View(result);
		}

		public ActionResult Export(IEnumerable<OrderModel> orders)
		{
			var service = new BricklinkService();

			var ordersWithShipping = orders
				.Where(x => x.Selected)
				.Select(x => service.GetOrderForCsv(x.OrderId.ToString()))
				.OrderBy(x => x.ShippingMethod)
				.ThenBy(x => x.Weight)
				.ThenBy(x => x.Name);

			var bytes = WriteCsvToMemory(ordersWithShipping);

			return File(bytes, "text/csv", DateTime.Now.ToString("yyyy-MM-dd") + " Bricklink Export.csv");
		}

		public byte[] WriteCsvToMemory(IEnumerable<OrderCsvModel> records)
		{
			using (var memoryStream = new MemoryStream())
			using (var streamWriter = new StreamWriter(memoryStream))
			using (var csvWriter = new CsvWriter(streamWriter, new CsvConfiguration(Thread.CurrentThread.CurrentCulture)))
			{
				csvWriter.WriteRecords(records);
				streamWriter.Flush();
				return memoryStream.ToArray();
			}
		}
	}
}