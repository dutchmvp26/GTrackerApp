using BLL.Services;
using GTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Xml.Linq;

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

        [BindProperty]
        public IFormFile UploadedImage { get; set; }



        // Load existing game
        public IActionResult OnGet(int id)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToPage("/Login");

            Game = _gameService.GetGameById(id);
            if (Game == null)
                return RedirectToPage("/Index");

            return Page();
        }


        // Save changes
        public async Task<IActionResult> OnPost()
        {
                ModelState.Remove("Game.AddedByUserID");
                ModelState.Remove("Game.Stars");    
            if (Game == null)
            {
                ModelState.AddModelError("", "No game provided.");
                return Page();
            }

            try
            {
                int? userId = HttpContext.Session.GetInt32("UserId");
                Game.AddedByUserID = userId;

                byte[] imageBytes = null;

                if (UploadedImage != null && UploadedImage.Length > 0)
                {
                    using var ms = new MemoryStream();
                    await UploadedImage.CopyToAsync(ms);
                    Game.BoxArt = ms.ToArray();
                }
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
