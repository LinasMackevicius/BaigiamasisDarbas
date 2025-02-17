using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using projektas.Data;
using projektas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace projektas.Pages.Statistics
{
    public class Monthly_ReportModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public Monthly_ReportModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<int> MonthlyConversationCounts { get; set; } = new List<int>();
        public List<double> MonthlyAverageSuccessRatings { get; set; } = new List<double>();
        public List<string> LastSixMonths { get; set; } = new List<string>();

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = HttpContext.Session.GetInt32("UserId") ?? 0;

            var sixMonthsAgo = DateTime.Now.AddMonths(-6);

            var sessions = await _context.SessionsList
                .Where(s => s.UserId == userId && s.SessionDate >= sixMonthsAgo)
                .Include(s => s.Conversations)
                .ToListAsync();

            var monthData = new Dictionary<string, (int peopleCount, double ratingTotal, int ratingCount)>();

            foreach (var session in sessions)
            {
                var monthKey = session.SessionDate.ToString("yyyy-MM");

                if (!monthData.ContainsKey(monthKey))
                {
                    monthData[monthKey] = (0, 0, 0);
                }

                monthData[monthKey] = (
                    monthData[monthKey].peopleCount + session.Conversations.Count,
                    monthData[monthKey].ratingTotal + session.Conversations.Sum(c => c.SuccessRating),
                    monthData[monthKey].ratingCount + session.Conversations.Count
                );
            }

            for (int i = 5; i >= 0; i--)
            {
                var monthDate = DateTime.Now.AddMonths(-i);
                var monthKey = monthDate.ToString("yyyy-MM");

                LastSixMonths.Add(monthDate.ToString("MMMM yyyy"));

                if (monthData.ContainsKey(monthKey))
                {
                    MonthlyConversationCounts.Add(monthData[monthKey].peopleCount);

                    double averageRating = monthData[monthKey].ratingCount > 0
                        ? monthData[monthKey].ratingTotal / monthData[monthKey].ratingCount
                        : 0;
                    MonthlyAverageSuccessRatings.Add(averageRating);
                }
                else
                {
                    MonthlyConversationCounts.Add(0);
                    MonthlyAverageSuccessRatings.Add(0);
                }
            }

            return Page();
        }
    }
}
