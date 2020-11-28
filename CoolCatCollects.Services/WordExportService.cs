using System.Linq;
using Xceed.Words.NET;

namespace CoolCatCollects.Services
{
	public class WordExportService
	{
		public string ExportRemarks(string[] allRemarks, string set, string path, int page)
		{
			allRemarks = allRemarks.Skip(190 * (page - 1)).ToArray();

			allRemarks = FillInBlanks(allRemarks);

			string filename = path + $"Remarks-{set}-{page}.docx";
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

			string[] FillInBlanks(string[] arr)
			{
				if (arr.Length < 189)
				{
					var lst = arr.ToList();

					while (lst.Count < 189)
					{
						lst.Add("");
					}

					arr = lst.ToArray();
				}

				return arr;
			}
		}
	}
}
