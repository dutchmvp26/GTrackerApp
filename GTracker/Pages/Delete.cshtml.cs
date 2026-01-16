using GTracker.Models;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GTracker.Pages
{
    public class DeleteModel : PageModel
    {
        private readonly GameService _gameService;

        public DeleteModel(GameService gameService)
        {
            _gameService = gameService;
        }

        [BindProperty]
        public Game? GameToDelete { get; set; }

        public IActionResult OnGet(int id)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToPage("/Login");

            var game = _gameService.GetGameById(id);
            if (game == null || game.AddedByUserID != userId.Value)
                return RedirectToPage("/Index");

            GameToDelete = game;

            return Page();
        }


        public IActionResult OnPost(int id)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToPage("/Login");
            try
            {
                _gameService.DeleteGame(id, userId.Value);
                return RedirectToPage("/Index");
            }
            catch (UnauthorizedAccessException)
            {
                return RedirectToPage("/Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error deleting game: {ex.Message}");
                return Page();
            }
        }
    }
}
