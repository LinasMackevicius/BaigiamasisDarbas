using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using projektas.Models;
using projektas.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace projektas.Pages.Notes
{
    public class InsightNotesModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public InsightNotesModel(ApplicationDbContext context)
        {
            _context = context;
            Note = new InsightNote(); // Prevent null reference
        }

        [BindProperty]
        public InsightNote Note { get; set; }

        public List<InsightNote> InsightNotesList { get; set; } = new();

        public void OnGet()
        {
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            InsightNotesList = _context.InsightNotes
                .Where(n => n.UserId == userId)
                .ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;

            if (Note.Id == 0)
            {
                Note.UserId = userId;
                _context.InsightNotes.Add(Note);
            }
            else
            {
                var existingNote = await _context.InsightNotes.FindAsync(Note.Id);
                if (existingNote != null && existingNote.UserId == userId)
                {
                    existingNote.Title = Note.Title;
                    existingNote.Content = Note.Content;
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;

            if (Note.Id == 0)
            {
                return RedirectToPage();
            }

            var noteToDelete = await _context.InsightNotes.FindAsync(Note.Id);
            if (noteToDelete != null && noteToDelete.UserId == userId)
            {
                _context.InsightNotes.Remove(noteToDelete);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }
    }
}
