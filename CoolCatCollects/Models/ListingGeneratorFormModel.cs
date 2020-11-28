using System.ComponentModel;
using System.Web;

namespace CoolCatCollects.Models
{
    public class ListingGeneratorFormModel
    {
        [DisplayName("What type of listing is this?")]
        public string Type { get; set; }
        [DisplayName("Title")]
        public string Title { get; set; }
        [DisplayName("Part Number")]
        public string Number { get; set; }
        [DisplayName("Condition")]
        public string Condition { get; set; }
        [DisplayName("Colour")]
        public string Colour { get; set; }
        [DisplayName("Colour Description")]
        public string ColourDescription { get; set; }
        [DisplayName("Pluralise")]
        public bool Plural { get; set; }

        public IHtmlString Html { get; set; }

        public ListingGeneratorFormModel NoHtml()
        {
            return new ListingGeneratorFormModel
            {
                Type = Type,
                Title = Title,
                Number = Number,
                Condition = Condition,
                ColourDescription = ColourDescription,
                Colour = Colour,
                Plural = Plural
            };
        }
    }
}