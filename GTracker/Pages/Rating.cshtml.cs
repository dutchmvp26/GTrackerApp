using GTracker.Models;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;

namespace GTracker.Pages
{
    public class RatingModel : PageModel
    {

        private readonly GameService _gameService;

        public RatingModel(GameService gameService)
        {
            _gameService = gameService;

        }

   
        public Game? Game { get; set; }

        [BindProperty]
        public Rating Rating { get; set; } = new Rating();

        public IActionResult OnGet(int id)
        {
            var allGames = _gameService.GetAllGames();
            Game = allGames.FirstOrDefault(g => g.Id == id);

            if (Game == null)
            {
                return RedirectToPage("/Index");
            }

            Rating.GameId = id;

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            int? userId = HttpContext.Session.GetInt32("UserId");
           

            if (userId == null)
            {
                ModelState.AddModelError("", "You must be logged in to rate.");
                return RedirectToPage("/Login");
            }

            Console.WriteLine($"GameId={Rating.GameId}, UserId={Rating.UserId}, Stars={Rating.Stars}");


            // Assign user + game
            Rating.UserId = userId.Value;

            try
                {
                _gameService.AddRating(Rating);
                return RedirectToPage("/Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error adding rating: {ex.Message}");
                return Page();
            }

        }
    }
}
