using BLL.Services;
using GTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace GTracker.tests
{
    [TestClass]
    public class GameServiceTests
    {
        //Test 3
        [TestMethod]

        public void GetGameById_InvalidId_ReturnsNull()
        {
         
            //Arrange
            GameService gameService = new GameService(new GameRepositoryMock());


            // Act
            Game? result = gameService.GetGameById(0);


            // Assert
            Assert.IsNull(result);
        }

        //Test 4
        [TestMethod]
        public void DeleteGame_ValidGameId_CallsRepositoryWithSameId()
        {

            //Arrange
            var repo = new GameRepositoryMock();
            var service = new GameService(repo);


            // Act

            service.DeleteGame(2);


            // Assert
            Assert.IsTrue(repo.DeleteGameCalled);
            Assert.AreEqual(2, repo.DeletedGameId);
        }


    }
}
    