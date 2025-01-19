using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

namespace projektas.Pages
{
    public class IndexModel : PageModel
    {
        public IActionResult OnGet()
        {
            // Check if the user is logged in
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                // If not logged in, redirect to the login page
                return RedirectToPage("/Account/Login");
            }

            // If logged in, allow access to the Index page
            return Page();
        }
    }
}
