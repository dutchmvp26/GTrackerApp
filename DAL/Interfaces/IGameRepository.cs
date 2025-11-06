using GTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IGameRepository
    {
        public List<Game> GetAllGames();
        Game? GetGameById(int id);
        public void DeleteGame(int gameId);
        public void AddGame(Game game);

        public void UpdateGame(Game game);

        List<Game> SearchGames(string? title, int? releaseYear, string? genre, string? platform, GameStatus? status);

    }
}
