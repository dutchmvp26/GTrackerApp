using BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GTracker.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly AuthService _authService;

        public RegisterModel(AuthService authService)
        {
            _authService = authService;
        }

        [BindProperty] public string Username { get; set; } = "";
        [BindProperty] public string Email { get; set; } = "";
        [BindProperty] public string Password { get; set; } = "";

        public string? ErrorMessage { get; set; }
        public string? SuccessMessage { get; set; }

        public void OnGet() { }

        public void OnPost()
        {
            try
            {
                _authService.Register(Username, Email, Password);
                SuccessMessage = "Account created! You can now log in.";
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }
    }
}
