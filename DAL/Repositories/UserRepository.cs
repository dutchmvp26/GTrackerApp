using DAL.Interfaces;
using GTracker.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;
        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public User? GetUserByUsername(string username)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            var cmd = new SqlCommand("SELECT * FROM [User] WHERE Username = @username", conn);
            cmd.Parameters.AddWithValue("@username", username);

            using var reader = cmd.ExecuteReader();
            if (!reader.Read()) return null;

            return new User
            {
                ID = (int)reader["ID"],
                Username = reader["Username"].ToString()!,
                Email = reader["Email"].ToString()!,
                PasswordHash = reader["Password"].ToString()!
            };
        }

        public User? GetUserByEmail(string email)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            var cmd = new SqlCommand("SELECT * FROM [User] WHERE Email = @email", conn);
            cmd.Parameters.AddWithValue("@email", email);

            using var reader = cmd.ExecuteReader();
            if (!reader.Read()) return null;

            return new User
            {
                ID = (int)reader["ID"],
                Username = reader["Username"].ToString()!,
                Email = reader["Email"].ToString()!,
                PasswordHash = reader["Password"].ToString()!
            };
        }

        public void AddUser(User user)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            var cmd = new SqlCommand(
                "INSERT INTO [User] (Username, Email, Password) VALUES (@username, @email, @password)",
                conn);
            cmd.Parameters.AddWithValue("@username", user.Username);
            cmd.Parameters.AddWithValue("@email", user.Email);
            cmd.Parameters.AddWithValue("@password", user.PasswordHash);
            cmd.ExecuteNonQuery();
        }
    }
}
