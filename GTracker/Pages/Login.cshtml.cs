using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BLL.Services;

namespace GTracker.Pages
{
    public class LoginModel : PageModel
    {
        private readonly AuthService _authService;
        public LoginModel(AuthService authService)
        {
            _authService = authService;
        }

        [BindProperty] public string Username { get; set; } = "";
        [BindProperty] public string Password { get; set; } = "";
        public string? ErrorMessage { get; set; }

        public void OnGet() { 
            HttpContext.Session.Clear();
        }


        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                ErrorMessage = "Please fill in the correct details.";
                return Page();
            }

            var user = _authService.Login(Username, Password);
            if (user == null)
            {
                ErrorMessage = "Invalid username or password.";
                return Page();
            }

            // store session cookies
            HttpContext.Session.SetInt32("UserId", user.ID);
            HttpContext.Session.SetString("Username", user.Username);

            return RedirectToPage("/Index");
        }
    }
}
