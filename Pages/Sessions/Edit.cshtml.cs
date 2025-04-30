using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using projektas.Data;
using projektas.Models;
using System.Threading.Tasks;
using System.Linq;

namespace projektas.Pages.Sessions
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Session Session { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Session = await _context.SessionsList
                .Include(s => s.Conversations)
                .FirstOrDefaultAsync(s => s.Id == id && s.UserId == HttpContext.Session.GetInt32("UserId"));

            if (Session == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var sessionToUpdate = await _context.SessionsList
                .Include(s => s.Conversations)
                .FirstOrDefaultAsync(s => s.Id == Session.Id && s.UserId == HttpContext.Session.GetInt32("UserId"));

            if (sessionToUpdate == null)
            {
                return NotFound();
            }

            // Update scalar session fields
            sessionToUpdate.Place = Session.Place;
            sessionToUpdate.Goals = Session.Goals;
            sessionToUpdate.SessionDate = Session.SessionDate;
            sessionToUpdate.SessionType = Session.SessionType;
            sessionToUpdate.TimeOfADayStart = Session.TimeOfADayStart;

            // Update conversations (basic example: update inline only)
            foreach (var conv in Session.Conversations)
            {
                var existing = sessionToUpdate.Conversations.FirstOrDefault(c => c.Id == conv.Id);
                if (existing != null)
                {
                    existing.PersonName = conv.PersonName;
                    existing.Duration = conv.Duration;
                    existing.SuccessRating = conv.SuccessRating;
                    existing.Comment = conv.Comment;
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToPage("Past_Sessions");
        }
    }
}
