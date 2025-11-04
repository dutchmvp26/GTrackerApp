using GTracker.Models;
using BLL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace GTracker.tests
{
    [TestClass]
    public class UnitTestGameService
    {
        GameService gameService = new GameService(new GameRepositoryMock());

        [TestMethod]
        public void GetAllGames()
        {
            // Arrange
            List<Game> expectedGames = new List<Game>();
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

            expectedGames.Add(game);
            
            // Act
            List<Game> actualGames = gameService.GetAllGames();


            // Assert
            Assert.AreEqual(expectedGames[0].Id, actualGames[0].Id);
            Assert.AreEqual(expectedGames[0].Title, actualGames[0].Title);
            Assert.AreEqual(expectedGames[0].releaseYear, actualGames[0].releaseYear);
            Assert.AreEqual(expectedGames[0].Genre, actualGames[0].Genre);
            Assert.AreEqual(expectedGames[0].boxArtUrl, actualGames[0].boxArtUrl);
            Assert.AreEqual(expectedGames[0].Platform, actualGames[0].Platform);
            Assert.AreEqual(expectedGames[0].Status, actualGames[0].Status);
            Assert.AreEqual(expectedGames[0].IsCustom, actualGames[0].IsCustom);
            Assert.AreEqual(expectedGames[0].AddedByUserID, actualGames[0].AddedByUserID);
            Assert.AreEqual(expectedGames[0].Notes, actualGames[0].Notes);
            Assert.AreEqual(expectedGames.Count, actualGames.Count);
        }
    }
}
