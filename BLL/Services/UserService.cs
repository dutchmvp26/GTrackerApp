using BLL.Interfaces;
using GTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class UserService
    {
        private readonly IUserRepository _repo;

        public UserService(IUserRepository repo) => _repo = repo;

        public User? GetUserById(int id) => _repo.GetUserById(id);

        public void UpdateProfilePicture(int userId, byte[] photo) =>
            _repo.UpdateProfilePicture(userId, photo);

        public void RemoveProfilePicture(int userId) =>
            _repo.RemoveProfilePicture(userId);
    }
}
