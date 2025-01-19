using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using projektas.Data;
using projektas.Models;
using System.Linq;

namespace projektas.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public LoginModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }

        public IActionResult OnPost()
        {
            // Find the user in the database
            var user = _context.Users.SingleOrDefault(u => u.Username == Username && u.Password == Password);

            if (user != null)
            {
                // Store the user's ID and Username in session to signify they're logged in
                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("Username", user.Username);

                Console.WriteLine($"Logged in as: {user.Username} with ID: {user.Id}"); // Debug message

                // Redirect to the Index page after successful login
                return RedirectToPage("/Index");
            }

            // Display an error message if login fails
            ErrorMessage = "Invalid username or password";
            return Page();
        }

    }
}
