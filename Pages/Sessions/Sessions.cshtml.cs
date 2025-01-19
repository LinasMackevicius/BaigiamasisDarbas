using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using projektas.Filters;

namespace projektas.Pages.Sessions
{
    [AuthorizeUser]
    public class SessionsModel : PageModel
    {
        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToPage("/Account/Login");
            }
            return Page();
        }

    }
}
