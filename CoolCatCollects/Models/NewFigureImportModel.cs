using CoolCatCollects.Core;

namespace CoolCatCollects.Models
{
    public class NewFigureImportModel
    {
        public string Theme { get; set; }
        public string SubTheme { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
        public decimal Price { get; set; }

        public string Title
        {
            get => SubTheme.IsEmpty() ?
                    $"Lego {Theme} {Name}" :
                    $"Lego {Theme} {SubTheme} {Name}";
        }
    }
}