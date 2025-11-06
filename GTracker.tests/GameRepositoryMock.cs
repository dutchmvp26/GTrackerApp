using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTracker.Models;
using Models;
using DAL.Interfaces;

namespace GTracker.tests
{
    public class GameRepositoryMock : IGameRepository
    {
        Game game = new Game()
        {
            Id = 2,
            Title = "Test Game 2",
            releaseYear = 2020,
            Genre = "Action",
            boxArtUrl = "http://example.com/boxart2.jpg",
            Platform = "PC",
            Status = GameStatus.Wishlist,
            IsCustom = false,
            AddedByUserID = 1,
            Notes = "Second test game"
        };

        public List<Game> GetAllGames()
        {
            return new List<Game>() { game };
        }

        public void DeleteGame(int gameId)
        {
            
        }

        public void AddGame(Game game)
        {

        }

        public List<Game> SearchGames(string? title, int? releaseYear, string? genre, string? platform, GameStatus? status)
        {
            return new List<Game> { game };

        }
    }
}
