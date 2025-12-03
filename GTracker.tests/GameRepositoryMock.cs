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

        public Rating? GetRatingById(int id)
        {
            throw new NotImplementedException();
        }


        public Game? GetGameById(int id)
        {
            if (id == game.Id)
            {
                return game;
            }
            return null;
        }

        public List<Game> GetAllGames()
        {
            return new List<Game>() { game };
        }

        public void DeleteGame(int gameId)
        {
            throw new NotImplementedException();
        }

        public void AddGame(Game game)
        {
            throw new NotImplementedException();
        }
        public void UpdateRating(Rating rating)
        {
            throw new NotImplementedException();
        }

       public void AddRating(Rating rating)
        {
            throw new NotImplementedException();
        }

        public List<Game> GetGamesByUserId(int userId)
        {
         throw new NotImplementedException();
                
        }

        public List<Game> SearchGames(int userId, string? title, int? releaseYear, string? genre, string? platform, GameStatus? status)
        {
            return new List<Game> { game };

        }

        public void UpdateGame(Game game)
        {
            throw new NotImplementedException();
        }
    }
}
