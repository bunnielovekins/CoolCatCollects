using System.Linq;
using Xceed.Words.NET;

namespace CoolCatCollects.Services
{
	public class WordExportService
	{
		public string ExportRemarks(string[] allRemarks, string set, string path)
		{
			if (allRemarks.Length < 189)
			{
				var lst = allRemarks.ToList();

				while(lst.Count < 189)
				{
					lst.Add("");
				}

				allRemarks = lst.ToArray();
			}

			string filename = path + $"output-{set}.docx";
			string template = path + "Avery_Template.docx";

			using (var templateDoc = DocX.Load(template))
			{
				for (int i = 0; i < 189; i++)
				{
					templateDoc.ReplaceText($"<<[Remarks[{i}]]>>", allRemarks[i]);
				}

				templateDoc.SaveAs(filename);
			}

			return filename;
		}
	}
}
