using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Interfaces;
using GTracker.Models;
using System.Security.Cryptography;

namespace BLL.Services
{
    public class AuthService
    {
        private readonly IUserRepository _userRepo;

        public AuthService(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public bool Register(string username, string email, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Username and password required.");

            // check existing username
            if (_userRepo.GetUserByUsername(username) != null)
                throw new Exception("Username is already taken.");

            // check existing email
            if (_userRepo.GetUserByEmail(email) != null)
                throw new Exception("Email already exists.");

            var hash = HashPassword(password);

            var newUser = new User
            {
                Username = username,
                Email = email,
                PasswordHash = hash
            };

            _userRepo.AddUser(newUser);
            return true;
        }

        public User? Login(string username, string password)
        {
            var user = _userRepo.GetUserByUsername(username);
            if (user == null) return null;

            return VerifyPassword(password, user.PasswordHash) ? user : null;
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            var hash = HashPassword(password);
            return hash == storedHash;
        }
    }
}
