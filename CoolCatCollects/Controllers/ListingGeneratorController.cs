using CoolCatCollects.Models;
using ExcelDataReader;
using RazorEngine;
using RazorEngine.Templating;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Linq;

namespace CoolCatCollects.Controllers
{
	public class ListingGeneratorController : Controller
	{
		// GET: ListingGenerator
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult Form()
		{
			var model = new ListingGeneratorFormModel();
			return View(model);
		}

		public ActionResult ViewTemplate(ListingGeneratorFormModel model)
		{
			return View($"~/Views/ListingGenerator/Templates/{model.Type}.cshtml", model);
		}

		public ActionResult FormResult(ListingGeneratorFormModel model)
		{
			model.Title = AddMinifigToTitle(model.Title);

			model.Html = Render();

			return View(model);

			IHtmlString Render()
			{
				var viewLoc = ViewLocation();
				var viewTemplate = GetViewContents(viewLoc);

				var str = !Engine.Razor.IsTemplateCached(viewTemplate, typeof(ListingGeneratorFormModel))
					? Engine.Razor.RunCompile(viewTemplate, viewTemplate, typeof(ListingGeneratorFormModel),
						model)
					: Engine.Razor.Run(viewTemplate, typeof(ListingGeneratorFormModel), model);

				return new HtmlString(str.Trim());
			}

			string GetViewContents(string viewTemplate)
			{
				return System.IO.File.ReadAllText(Server.MapPath(viewTemplate));
			}

			string ViewLocation()
			{
				return $"~/Views/ListingGenerator/Templates/{model.Type}.cshtml";
			}

			string AddMinifigToTitle(string title)
			{
				if (!model.Type.Contains("Fig"))
				{
					return title;
				}
				if (title.Contains("Minifigure"))
				{
					return title;
				}

				return title + " Minifigure";
			}
		}

		#region upload

		[HttpPost]
		public ActionResult UploadNewFigures(HttpPostedFileBase file)
		{
			var dataSet = GetDataSetFromFile(file.InputStream);
			var model = DatasetToModel(dataSet);

			return View(model);

			DataSet GetDataSetFromFile(Stream fileStream)
			{
				using (var reader = ExcelReaderFactory.CreateReader(fileStream))
				{
					return reader.AsDataSet();
				}
			}

			IEnumerable<NewFigureImportModel> DatasetToModel(DataSet ds)
			{
				var arr = new DataRow[ds.Tables[0].Rows.Count];
				ds.Tables[0].Rows.CopyTo(arr, 0);
				return arr.Skip(1).Select(x => new NewFigureImportModel
				{
					Theme = x.ItemArray[0].ToString(),
					SubTheme = x.ItemArray[1].ToString(),
					Name = x.ItemArray[2].ToString(),
					Number = x.ItemArray[3].ToString(),
					Price = ParseDecimal(x.ItemArray[4])
				}).ToList();
			}

			decimal ParseDecimal(object obj)
			{
				if (decimal.TryParse(obj.ToString(), out decimal result))
				{
					return result;
				}
				return 0;
			}
		}

		[HttpPost]
		public ActionResult UploadUsedFigures(HttpPostedFileBase file)
		{
			var dataSet = GetDataSetFromFile(file.InputStream);
			var model = DatasetToModel(dataSet);

			return View(model);

			DataSet GetDataSetFromFile(Stream fileStream)
			{
				using (var reader = ExcelReaderFactory.CreateReader(fileStream))
				{
					return reader.AsDataSet();
				}
			}

			IEnumerable<UsedFigureImportModel> DatasetToModel(DataSet ds)
			{
				var arr = new DataRow[ds.Tables[0].Rows.Count];
				ds.Tables[0].Rows.CopyTo(arr, 0);
				return arr.Skip(1).Select(x => new UsedFigureImportModel
				{
					Theme = x.ItemArray[0].ToString(),
					SubTheme = x.ItemArray[1].ToString(),
					Name = x.ItemArray[2].ToString(),
					Number = x.ItemArray[3].ToString(),
					Complete = x.ItemArray[4].ToString(),
					Condition = x.ItemArray[5].ToString(),
					Price = ParseDecimal(x.ItemArray[6])
				}).ToList();
			}
		}

		private decimal ParseDecimal(object obj)
		{
			if (decimal.TryParse(obj.ToString(), out decimal result))
			{
				return result;
			}
			return 0;
		}

		#endregion
		#region files

		public ActionResult UsedTemplate()
		{
			DownloadFile("~/Files/UsedImportTemplate.xlsx", "UsedFigsImportTemplate.xlsx");

			return Content("");
		}

		public ActionResult NewTemplate()
		{
			DownloadFile("~/Files/NewImportTemplate.xlsx", "NewFigsImportTemplate.xlsx");

			return Content("");
		}

		/// <summary>
		/// Downloads a file
		/// </summary>
		/// <param name="filePath">Path to file pre-mappath</param>
		/// <param name="fileName">Filename when downloading</param>
		private void DownloadFile(string filePath, string fileName)
		{
			Response.ClearContent();
			Response.Clear();
			Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
			Response.AddHeader("Content-Disposition", $"attachment; filename={fileName};");
			Response.TransmitFile(Server.MapPath(filePath));
			Response.Flush();
			Response.End();
		}

		#endregion
	}
}