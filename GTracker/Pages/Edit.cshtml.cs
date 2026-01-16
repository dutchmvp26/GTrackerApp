    using BLL.Services;
    using GTracker.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using System.Xml.Linq;
    using System.Drawing;

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
            public IFormFile? UploadedImage { get; set; }

            public IFormFile? UploadedScreenImage { get; set; }



        // Load existing game
        public IActionResult OnGet(int id)
            {
                int? userId = HttpContext.Session.GetInt32("UserId");

                if (userId == null)
                    return RedirectToPage("/Login");

            var game = _gameService.GetGameById(id);
            if (game == null || game.AddedByUserID != userId.Value)
                return RedirectToPage("/Index");

             Game = game;

            return Page();
            }


            // Save changes
            public async Task<IActionResult> OnPost()
            {
                    ModelState.Remove("Game.AddedByUserID");
                    ModelState.Remove("Game.Stars");

                //set up existing game, copy over editable fields
                var existingGame = _gameService.GetGameById(Game.Id);
                if (existingGame == null)
                    return NotFound();
                existingGame.Title = Game.Title;
                existingGame.Platform = Game.Platform;
                existingGame.releaseYear = Game.releaseYear;
                existingGame.Genre = Game.Genre;
                existingGame.Status = Game.Status;
                existingGame.Notes = Game.Notes;


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
                        existingGame.BoxArt = ms.ToArray();
                    }
                if (UploadedScreenImage != null && UploadedScreenImage.Length > 0)
                {
                    using var imageStreamScreen = UploadedScreenImage.OpenReadStream();
                    using var imageScreen = Image.FromStream(imageStreamScreen);

                    imageStreamScreen.Position = 0;


                    using var ms = new MemoryStream();
                    await UploadedScreenImage.CopyToAsync(ms);
                    existingGame.Screenshot = ms.ToArray();
                }
                _gameService.UpdateGame(existingGame);
                    return RedirectToPage("./Index");

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error updating game: {ex.Message}");
                    return Page();
                }
            }
        }
    }
