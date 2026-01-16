using GTracker.Models;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GTracker.Pages
{
    public class ViewModel : PageModel
    {
        private readonly GameService _gameService;

        public ViewModel(GameService gameService)
        {
            _gameService = gameService;
        }

        public Game? Game { get; set; }

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
        }
    }
