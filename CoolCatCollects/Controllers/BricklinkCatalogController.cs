using CoolCatCollects.Bricklink;
using CoolCatCollects.Bricklink.Models;
using CoolCatCollects.Services;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Serialization;

namespace CoolCatCollects.Controllers
{
	public class BricklinkCatalogController : Controller
	{
		private readonly BricklinkService _service;

		public BricklinkCatalogController()
		{
			_service = new BricklinkService();
		}

		public ActionResult Index()
		{
			return View();
		}

		public ActionResult PartListFromSet(string set, bool debug = false)
		{
			if (string.IsNullOrEmpty(set))
			{
				return RedirectToAction("Index");
			}

			if (!set.Contains("-"))
			{
				set += "-1";
			}

			var parts = _service.GetPartsFromSet(set, debug: debug);

			return View(model: parts);
		}

		public ActionResult PartsByRemark(string set)
		{
			if (string.IsNullOrEmpty(set))
			{
				return RedirectToAction("Index");
			}

			if (!set.Contains("-"))
			{
				set += "-1";
			}

			var parts = _service.GetPartsFromSet(set, true);

			return View(model: parts);
		}

		public ActionResult ExportRemarks(string remarks, string set)
		{
			var allRemarks = remarks.Split(',');

			var service = new WordExportService();
			var filename = service.ExportRemarks(allRemarks, set, Server.MapPath("~/App_Data/"));

			byte[] content = System.IO.File.ReadAllBytes(filename);
			Response.ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
			Response.AddHeader("content-disposition", "attachment; filename=output-" + set + ".docx");
			Response.BufferOutput = true;
			Response.OutputStream.Write(content, 0, content.Length);
			Response.End();

			return Content("");
		}

		[HttpPost]
		public ActionResult UpdateDatabase(BLXMLItem[] items)
		{
			_service.UpdateInventoryForParts(items.Select(x => new BricklinkService.MiniPartModel
			{
				Number = x.ITEMID,
				ColourId = x.COLOR,
				Condition = "N",
				ItemType = GetItemTypeFromChar(x.ITEMTYPE)
			}));

			return Content("");

			string GetItemTypeFromChar(string str)
			{
				switch (str)
				{
					case "S":
						return "SET";
					case "P":
						return "PART";
					case "M":
						return "MINIFIG";
					case "I":
						return "INSTRUCTIONS";
				}

				return str;
			}
		}

		[HttpPost]
		public ActionResult ExportXml(BLXMLItem[] items)
		{
			var obj = new BLXmlRoot
			{
				Items = items.Where(x => x.INCLUDE == "on").ToArray()
			};

			using (var stringwriter = new System.IO.StringWriter())
			{
				var serializer = new XmlSerializer(obj.GetType());
				serializer.Serialize(stringwriter, obj);

				return Content(stringwriter.ToString(), "application/xml");
			}
		}

		[HttpPost]
		public ActionResult GetSubset(string number, int colour, string type)
		{
			var model = _service.GetPartsFromSet(number, type: type, colourId: colour);

			return Json(model);
		}

		[Serializable]
		[XmlType(AnonymousType = true, TypeName = "INVENTORY")]
		[XmlRoot(Namespace = "", IsNullable = false, ElementName = "INVENTORY")]
		public class BLXmlRoot
		{
			[XmlElement("ITEM")]
			public BLXMLItem[] Items { get; set; }
		}

		[Serializable]
		[XmlType(AnonymousType = true, TypeName = "ITEM")]
		public class BLXMLItem
		{
			public string ITEMID { get; set; }
			public int COLOR { get; set; }
			public int CATEGORY { get; set; }
			public string ITEMTYPE { get; set; }
			public int QTY { get; set; }
			private string p;
			[XmlIgnore]
			public string pricestr
			{
				get
				{
					return p;
				}
				set
				{
					p = value;
					PRICE = decimal.Parse(p);
				}
			}
			public decimal PRICE { get; set; }
			public string CONDITION { get; set; }
			public string REMARKS { get; set; }
			[XmlIgnore]
			public string INCLUDE { get; set; }
		}
	}
}