using CoolCatCollects.Bricklink;
using CoolCatCollects.Bricklink.Models;
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

		// GET: BricklinkCatalog
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult PartListFromSet(string set)
		{
			if (string.IsNullOrEmpty(set))
			{
				return RedirectToAction("Index");
			}

			if (!set.Contains("-"))
			{
				set += "-1";
			}

			var parts = _service.GetPartsFromSet(set, forceUpdate: true);

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