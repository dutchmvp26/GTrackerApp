using GTracker.Models;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GTracker.Pages
{
    public class CreateModel : PageModel
    {
        private readonly GameService _gameService;

        public CreateModel(GameService gameService)
        {
            _gameService = gameService;

        }

        [BindProperty]
        public Game NewGame { get; set; } = new Game();

        public IActionResult OnGet()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToPage("/Login");
            // set defaults if needed
            NewGame.Status = GameStatus.CurrentlyPlaying;

            return Page();
        }

        public IActionResult OnPost()
        {
                int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToPage("/Login");

            // assign owner:
            NewGame.AddedByUserID = userId;

            try
            {
                _gameService.AddGame(NewGame);
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error adding game: {ex.Message}");
                return Page();
            }
        }
    }
}
