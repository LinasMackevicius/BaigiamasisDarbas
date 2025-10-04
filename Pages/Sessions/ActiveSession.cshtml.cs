using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using projektas.Data;
using projektas.Models;

namespace projektas.Pages.Sessions
{
    public class ActiveSessionModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ActiveSessionModel(ApplicationDbContext context)
        {
            _context = context;
            Session = new Session(); // Ensure Session is never null
            ConversationsJson = "[]"; // Default value for JSON string
        }

        [BindProperty]
        public Session Session { get; set; }

        [BindProperty]
        public string ConversationsJson { get; set; }  // JSON string for conversations

        public void OnGet()
        {
            if (Session.SessionDate == DateTime.MinValue)
            {
                Session.SessionDate = DateTime.Now; // Ensure SessionDate is set correctly
            }

            if (Session.TimeOfADayStart == default)
            {
                Session.TimeOfADayStart = TimeOnly.FromDateTime(DateTime.Now); // Set to current time
            }
        }

        public async Task<IActionResult> OnPostWriteSessionInfoToDatabaseAsync()
        {
            if (!ModelState.IsValid)
            {
                ConversationsJson = ConversationsJson ?? "[]";
                return Page();
            }

            // Assign UserId before saving
            Session.UserId = HttpContext.Session.GetInt32("UserId") ?? 0;

            // Add session to database
            _context.SessionsList.Add(Session);
            await _context.SaveChangesAsync();

            // Process conversations if there are any
            if (!string.IsNullOrEmpty(ConversationsJson))
            {
                var conversations = JsonConvert.DeserializeObject<List<ConversationInputModel>>(ConversationsJson) ?? new List<ConversationInputModel>();

                foreach (var conversation in conversations)
                {
                    var newConversation = new Conversation
                    {
                        PersonName = conversation.PersonName,
                        Duration = conversation.Duration,
                        SuccessRating = conversation.SuccessRating,
                        Comment = conversation.Comment,
                        SessionId = Session.Id, // Link conversation to the saved session
                    };
                    _context.Conversations.Add(newConversation);
                }
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/Sessions/ActiveSession");
        }
    }
}

