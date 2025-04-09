using Microsoft.AspNetCore.Mvc.RazorPages;
using projektas.Models;
using System.Collections.Generic;

namespace projektas.Pages.Notes
{
    public class GeneralNotesModel : PageModel
    {
        public List<GeneralNote> Notes { get; set; }

        // Sample data, which you can replace with real database data later
        public void OnGet()
        {
            Notes = new List<GeneralNote>
            {
                new GeneralNote { Title = "Insight 1", Content = "This is content for insight 1." },
                new GeneralNote { Title = "Insight 2", Content = "This is content for insight 2." },
                new GeneralNote { Title = "Insight 3", Content = "This is content for insight 3." }
            };
        }

        // This method handles saving the updated note content
        public void OnPostSave(int index, string content)
        {
            Notes[index].Content = content;
        }
    }
}
