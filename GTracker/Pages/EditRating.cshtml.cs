using GTracker.Models;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;

namespace GTracker.Pages
{
    public class EditRatingModel : PageModel
    {

        private readonly GameService _gameService;

        public EditRatingModel(GameService gameService)
        {
            _gameService = gameService;

        }


        public Game? Game { get; set; }

        [BindProperty]
        public Rating Rating { get; set; } = new Rating();



        public IActionResult OnGet(int gameId, int? ratingId)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToPage("/Index");
            }

            if (!ratingId.HasValue)
            {
                return RedirectToPage("/Index");
            }

            var game = _gameService.GetGameById(gameId);
            if (game == null || game.AddedByUserID != userId.Value)
            {
                return RedirectToPage("/Index");
            }

            var rating = _gameService.GetRatingById(ratingId.Value);
            if (rating == null)
            {
                return RedirectToPage("/Index");
            }

            Game = game;
            Rating = rating;

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
                _gameService.UpdateRating(Rating);
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
