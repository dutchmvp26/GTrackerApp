    using DAL.Interfaces;
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

            public List<Game> GetAllGames() => _repo.GetAllGames();

            public void DeleteGame(int gameId)
            {
                if (gameId <= 0)
                    throw new ArgumentException("Invalid game ID.");

                _repo.DeleteGame(gameId);
            }
        // balls balls big fat balls


            public void AddGame(Game game)
            {
                if (string.IsNullOrWhiteSpace(game.Title))
                    throw new ArgumentException("Game title cannot be empty");

                if (string.IsNullOrWhiteSpace(game.Platform))
                    throw new ArgumentException("Platform cannot be empty");

                _repo.AddGame(game);
            }
        }
    }
