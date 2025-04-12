using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using projektas.Models;
using projektas.Data;
using System;

namespace projektas.Pages.Notes
{
    public class GeneralNotesModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public GeneralNotesModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public GeneralNote? Note { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid || Note == null)
            {
                return Page();
            }

            _context.GeneralNotes.Add(Note);
            _context.SaveChanges(); // inserts into DB

            return RedirectToPage(); // refresh the page
        }
    }
}
