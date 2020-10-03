using CoolCatCollects.Core;

namespace CoolCatCollects.Models
{
    public class UsedFigureImportModel
    {
        public string Theme { get; set; }
        public string SubTheme { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
        public string Complete { get; set; }
        public string Condition { get; set; }
        public decimal Price { get; set; }

        public string Title
        {
            get => SubTheme.IsEmpty() ?
                    $"Lego {Theme} {Name} {Complete}" :
                    $"Lego {Theme} {SubTheme} {Name} {Complete}";
        }
    }
}