using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using projektas.Models;

namespace projektas.Pages.Notes
{
    public class GeneralNotesModel : PageModel
    {
        [BindProperty]
        public GeneralNote Note { get; set; }

        public void OnGet()
        {
            // Nothing yet
        }

        public void OnPost()
        {
            // For now just store it temporarily or debug
            var title = Note?.Title;
            var content = Note?.Content;
            // Later you can save this to a DB
        }
    }
}
