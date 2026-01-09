using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTracker.Models;

namespace BLL.Interfaces
{
    public interface IUserRepository
    {
        User? GetUserByUsername(string username);
        void AddUser(User user);

        User? GetUserById(int id);

        User? GetUserByEmail(string email);

        void UpdateProfilePicture(int userId, byte[] picture);
        void RemoveProfilePicture(int userId);
    }
}

