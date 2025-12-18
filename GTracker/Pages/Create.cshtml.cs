using BLL.Services;
using GTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Drawing;

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

        [BindProperty]
        public IFormFile? UploadedImage { get; set; }

        public IActionResult OnGet()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToPage("/Login");
            // set defaults if needed
            NewGame.Status = GameStatus.CurrentlyPlaying;

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {

                int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToPage("/Login");

            // assign owner:
            NewGame.AddedByUserID = userId;

            try
            {
                byte[] imageBytes = null;

                if (UploadedImage != null && UploadedImage.Length > 0)
                {
                    using var imageStream = UploadedImage.OpenReadStream();
                    using var image = Image.FromStream(imageStream);

                    if (image.Width > 400 || image.Height > 400)
                    {
                        ModelState.AddModelError(
                            "UploadedImage",
                            "Image must be 400 × 400 pixels or smaller."
                        );
                        return Page();
                    }

                    imageStream.Position = 0;


                    using var ms = new MemoryStream();
                    await UploadedImage.CopyToAsync(ms);
                    NewGame.BoxArt = ms.ToArray();
                }
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
