using GTracker.Models;
using LOG.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

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

        public void OnGet()
        {
            Games = _gameService.GetAllGames(); 
        }
    }
}
