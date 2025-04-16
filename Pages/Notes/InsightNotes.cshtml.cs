using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using projektas.Models;
using projektas.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Reflection.Metadata;

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

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Note.Id == 0)
            {
                // Add new
                _context.InsightNotes.Add(Note);
            }
            else
            {
                // Edit existing
                var existingNote = await _context.InsightNotes.FindAsync(Note.Id);
                if (existingNote != null)
                {
                    existingNote.Title = Note.Title;
                    existingNote.Content = Note.Content;
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToPage();
        }

    }
}
