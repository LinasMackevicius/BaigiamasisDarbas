using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using projektas.Data;
using projektas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace projektas.Pages.Sessions
{
    public class Past_SessionsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public Past_SessionsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Session> SessionsList { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? FilterDate { get; set; }  // Filter for date

        [BindProperty(SupportsGet = true)]
        public string FilterPlace { get; set; }    // Filter for place

        public async Task OnGetAsync()
        {
            var query = _context.SessionsList
                .Include(s => s.Conversations)
                .Include(s => s.InsightNotes)
                .Where(s => s.UserId == HttpContext.Session.GetInt32("UserId")) // Only the logged-in user's sessions
                .OrderByDescending(s => s.SessionDate)
                .AsQueryable();

            // Apply Date filter
            if (FilterDate.HasValue)
            {
                query = query.Where(s => s.SessionDate.Date == FilterDate.Value.Date);
            }

            // Apply Place filter
            if (!string.IsNullOrWhiteSpace(FilterPlace))
            {
                query = query.Where(s => EF.Functions.Like(s.Place, $"%{FilterPlace}%"));
            }

            SessionsList = await query.ToListAsync();
        }
    }
}
