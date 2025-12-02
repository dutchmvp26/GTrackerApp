using BLL.Services;
using GTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GTracker.Pages
{
    public class EditModel : PageModel
    {
        private readonly GameService _gameService;

        public EditModel(GameService gameService)
        {
            _gameService = gameService;
        }

        [BindProperty]
        public Game? Game { get; set; }

        // Load existing game
        public IActionResult OnGet(int id)
        {
            Game = _gameService.GetGameById(id);
            if (Game == null)
                return RedirectToPage("/Index");

            return Page();
        }

        // Save changes
        public IActionResult OnPost()
        {
            if (Game == null)
            {
                ModelState.AddModelError("", "No game provided.");
                return Page();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                int? userId = HttpContext.Session.GetInt32("UserId");
                Game.AddedByUserID = userId;

                _gameService.UpdateGame(Game);
                return RedirectToPage("/Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error updating game: {ex.Message}");
                return Page();
            }
        }
    }
}
