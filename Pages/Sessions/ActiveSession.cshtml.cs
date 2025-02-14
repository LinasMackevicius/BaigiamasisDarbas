using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using projektas.Data;
using projektas.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace projektas.Pages.Sessions
{
    public class ActiveSessionModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ActiveSessionModel(ApplicationDbContext context)
        {
            _context = context;

            // Initialize SessionDate with the current date
            SessionDate = DateTime.Now;
        }

        [BindProperty]
        public DateTime SessionDate { get; set; }

        [BindProperty]
        public SessionType SessionType { get; set; }

        [BindProperty]
        public TimeOnly TimeOfADayStart { get; set; }

        [BindProperty]
        public string Place { get; set; }

        [BindProperty]
        public string Goals { get; set; }

        [BindProperty]
        public string ConversationsJson { get; set; }  // JSON string for conversations

        public void OnGet()
        {
            if (SessionDate == DateTime.MinValue)
            {
                SessionDate = DateTime.Now;
            }
        }

        public async Task<IActionResult> OnPostSaveSessionAsync()
        {
            // why is this checking only the place??
            if (string.IsNullOrEmpty(Place))
            {
                ModelState.AddModelError("", "Place cannot be empty.");
                return Page();
            }

            Session session = new Session
            {   SessionType = SessionType,
                TimeOfADayStart = TimeOfADayStart,
                Date = SessionDate,
                Place = Place,
                Goals = Goals,
                UserId = HttpContext.Session.GetInt32("UserId") ?? 0
            };

            _context.SessionsList.Add(session);
            await _context.SaveChangesAsync();

            if (!string.IsNullOrEmpty(ConversationsJson))
            {
                var conversations = JsonConvert.DeserializeObject<List<ConversationInputModel>>(ConversationsJson);
                foreach (var conversation in conversations)
                {
                    var newConversation = new Conversation
                    {
                        PersonName = conversation.PersonName,
                        Duration = conversation.Duration,
                        SuccessRating = conversation.SuccessRating,
                        Comment = conversation.Comment,
                        SessionId = session.Id,
                        Date = SessionDate, // Set the conversation date
                        UserId = session.UserId // Set the conversation user ID
                    };
                    _context.Conversations.Add(newConversation);
                }
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/Sessions/ActiveSession");
        }

        public async Task<IActionResult> OnPostEndSessionAsync()
        {
            Console.WriteLine("OnPostEndSessionAsync called");
            await OnPostSaveSessionAsync();

            // Reset for a new session
            SessionDate = DateTime.Now;
            Place = string.Empty;
            Goals = string.Empty;
            ConversationsJson = "[]";

            return RedirectToPage("/Sessions/Active_Session");
        }
    }
}
