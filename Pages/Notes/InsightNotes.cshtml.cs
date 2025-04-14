using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using projektas.Models;
using projektas.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace projektas.Pages.Notes
{
    public class InsightNotesModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public InsightNotesModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public InsightNote? Note { get; set; }

        public List<InsightNote>? InsightNotesList { get; set; } = new();

        public void OnGet()
        {
            InsightNotesList = _context.InsightNotes
                .ToList();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid || Note == null)
            {
                return Page();
            }
            _context.InsightNotes.Add(Note);
            _context.SaveChanges();

            return RedirectToPage();
        }
    }
}
