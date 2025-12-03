    using GTracker.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using BLL.Services;
    using System.Collections.Generic;

    namespace GTracker.Pages
    {
        public class IndexModel : PageModel
        {
            private readonly GameService _gameService;
            private readonly ILogger<IndexModel> _logger;

            public List<Game> Games { get; set; } = new();

            public IndexModel(GameService gameService, ILogger<IndexModel> logger)
            {
                _gameService = gameService;
                _logger = logger;
            }

            [BindProperty(SupportsGet = true)]
            public string? Title { get; set; }

            [BindProperty(SupportsGet = true)]
            public int? ReleaseYear { get; set; }

            [BindProperty(SupportsGet = true)]
            public string? Genre { get; set; }

            [BindProperty(SupportsGet = true)]
            public string? Platform { get; set; }

            //
            // Filter by status
            [BindProperty(SupportsGet = true)]
            public GameStatus? Status { get; set; }

            public void OnGet()
            {
                int? userId = HttpContext.Session.GetInt32("UserId");
          
                var username = HttpContext.Session.GetString("Username");

            if (username == null || userId == null)
            {
                    Response.Redirect("/Login");
                    return;
                }

            Games = _gameService.GetGamesForUser(userId.Value);

            if (!string.IsNullOrWhiteSpace(Title) ||
                    ReleaseYear.HasValue ||
                    !string.IsNullOrWhiteSpace(Genre) ||
                    !string.IsNullOrWhiteSpace(Platform) ||
                    Status.HasValue)
                {
                    Games = _gameService.SearchGames(userId.Value, Title, ReleaseYear, Genre, Platform, Status);
                }
            }
        }
    }
