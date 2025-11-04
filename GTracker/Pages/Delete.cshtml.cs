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
            var allGames = _gameService.GetAllGames();
            GameToDelete = allGames.FirstOrDefault(g => g.Id == id);

            if (GameToDelete == null)
                return RedirectToPage("/Index");

            return Page();
        }

        public IActionResult OnPost(int id)
        {
            try
            {
                _gameService.DeleteGame(id);
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
