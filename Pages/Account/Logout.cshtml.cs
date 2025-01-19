using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace projektas.Pages.Account
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            // Clear the session to log out the user
            HttpContext.Session.Clear();

            // Redirect to the login page
            return RedirectToPage("/Account/Login");
        }
    }
}
