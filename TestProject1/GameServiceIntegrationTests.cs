using System;
using Xunit;
using BLL.Services;
using DAL.Repositories;
using BLL.Interfaces;

namespace GTracker.IntegrationTests
{
    public class GameServiceIntegrationTests
    {
        private readonly GameService _gameService;

        public GameServiceIntegrationTests()
        {
            TestDb.Reset();

            var gameRepository = new GameRepository(TestDb.Conn);

            _gameService = new GameService(gameRepository);
        }

        [Fact]
        public void DeleteGame_RemovesGameFromDatabase()
        {
            // Arrange
            int userId = TestDb.InsertUser();
            int gameId = TestDb.InsertGame(addedByUserId: userId, title: "Delete Me");

            // Act
            _gameService.DeleteGame(gameId, userId);

            // Assert
            var deleted = _gameService.GetGameById(gameId);
            Assert.Null(deleted);
        }

        [Fact]
        public void DeleteGame_InvalidGameId_ThrowsArgumentException()
        {
            TestDb.Reset();

            int userId = TestDb.InsertUser();

            Assert.Throws<ArgumentException>(() =>
                _gameService.DeleteGame(99999, userId)
            );
        }

    }
}
