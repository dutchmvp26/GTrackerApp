    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using BLL.Services;
    using BLL.Interfaces;
    using GTracker.Models;
    using Moq;
    using System.Security.Cryptography;

namespace GTracker.tests
{
    [TestClass]
    public class AuthServiceTests
    {
        private string HashPasswordForTests(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        //Test 1
        [TestMethod]
        public void Login_ValidCredentials_ReturnsUser()
        {
            // Arrange
            var usermockRepo = new Mock<IUserRepository>();

            var username = "testuser";

            var password = "password123";

            var fakeUser = new User
            {
                ID = 1,
                Username = username,
                Email = "test@gmail.com",
                PasswordHash = HashPasswordForTests(password)
            };

            usermockRepo
                .Setup(r => r.GetUserByUsername(username))
                .Returns(fakeUser);

            var authService = new AuthService(usermockRepo.Object);
            // Act
            var result = authService.Login(username, password);
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(username, result.Username);

            usermockRepo.Verify(r => r.GetUserByUsername(username), Times.Once);
        }

        //Test 2
        [TestMethod]

        public void Login_InvalidCredentials_ReturnsNull()
        {
            // Arrange

            var usermockRepo = new Mock<IUserRepository>();

            var username = "testuser";
            var password = "wrongpassword";
            var notrealUser = new User
            {
                ID = 1,
                Username = username,
                PasswordHash = HashPasswordForTests("hashed")

            };
            usermockRepo.Setup(r => r.GetUserByUsername(username)).Returns(notrealUser);

            var AuthService = new AuthService(usermockRepo.Object);


            // Act
            var result = AuthService.Login(username, password);

            // Assert
            Assert.IsNull(result);
        }

    }
}
