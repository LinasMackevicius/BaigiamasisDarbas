using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using projektas.Data;
using projektas.Models;
using System.Linq;
using System.Threading.Tasks;

namespace projektas.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public RegisterModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            // Check if the username already exists
            if (_context.Users.Any(u => u.Username == Username))
            {
                ErrorMessage = "Username already exists. Please choose a different username.";
                return Page();
            }

            // Create a new user
            var user = new User
            {
                Username = Username,
                Password = Password
            };

            // Save the user to the database
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Redirect to the login page after successful registration
            return RedirectToPage("/Account/Login");
        }
    }
}
