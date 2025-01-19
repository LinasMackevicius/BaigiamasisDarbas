using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace projektas.Filters
{
    public class AuthorizeUserAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine("AuthorizeUser filter triggered");

            var session = context.HttpContext.Session;
            if (session.GetInt32("UserId") == null) // Check if UserId is set in session
            {
                // Redirect to login page if not logged in
                context.Result = new RedirectToPageResult("/Account/Login");
            }
            base.OnActionExecuting(context);
        }

    }
}
