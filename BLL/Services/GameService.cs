    using BLL.Interfaces;
    using GTracker.Models;
    using System;
    using System.Collections.Generic;

    namespace BLL.Services
    {
        public class GameService
        {

            private readonly IGameRepository _repo;

            public GameService(IGameRepository repo)
            {
                _repo = repo;
            }
        
        public Game? GetGameById(int id)              
        {
            if (id <= 0) return null;
            return _repo.GetGameById(id);
        }
        public List<Game> GetAllGames() => _repo.GetAllGames();

            public void DeleteGame(int gameId)
            {
                if (gameId <= 0)
                    throw new ArgumentException("Invalid game ID.");

                _repo.DeleteGame(gameId);
            }
        public List<Game> SearchGames(int userId, string? title, int? releaseYear, string? genre, string? platform, GameStatus? status)
        {
            return _repo.SearchGames(userId, title, releaseYear, genre, platform, status);
        }

        public void AddRating(Rating rating)
        {
            try
            {
                _repo.AddRating(rating);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to add rating", ex);
            }

        }

        public void UpdateRating(Rating rating)
        {
            try
            {
                _repo.UpdateRating(rating);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to update rating", ex);
            }

        }

        public Rating? GetRatingById(int id)
        {
            return _repo.GetRatingById(id);
        }


        public void AddGame(Game game)
            {
                if (string.IsNullOrWhiteSpace(game.Title))
                    throw new ArgumentException("Game title cannot be empty");

                if (string.IsNullOrWhiteSpace(game.Platform))
                    throw new ArgumentException("Platform cannot be empty");

                _repo.AddGame(game);
            }

        public List<Game> GetGamesForUser(int userId)
        {
            return _repo.GetGamesByUserId(userId);
        }


        public void UpdateGame(Game game)             
        {
            if (game.Id <= 0) throw new ArgumentException("Invalid game ID.");
            if (string.IsNullOrWhiteSpace(game.Title)) throw new ArgumentException("Title is required.");
            if (string.IsNullOrWhiteSpace(game.Platform)) throw new ArgumentException("Platform is required.");

            _repo.UpdateGame(game);
        }
    }
    }
