using BLL.Interfaces;
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

        public User? GetUserById(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            var cmd = new SqlCommand(
                "SELECT * FROM [User] WHERE ID = @id",
                conn);

            cmd.Parameters.AddWithValue("@id", id);

            using var reader = cmd.ExecuteReader();
            if (!reader.Read()) return null;

            return new User
            {
                ID = (int)reader["ID"],
                Username = reader["Username"].ToString()!,
                Email = reader["Email"].ToString()!,
                PasswordHash = reader["Password"].ToString()!,
                PFP = reader["PFP"] != DBNull.Value
                    ? (byte[])reader["PFP"]
                    : null
            };
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

        public void UpdateProfilePicture(int userId, byte[] picture)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            var cmd = new SqlCommand(
                "UPDATE [User] SET PFP = @pic WHERE ID = @id",
                conn);

            cmd.Parameters.Add("@pic", System.Data.SqlDbType.VarBinary).Value = picture;
            cmd.Parameters.AddWithValue("@id", userId);

            cmd.ExecuteNonQuery();
        }

        public void RemoveProfilePicture(int userId)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            var cmd = new SqlCommand(
                "UPDATE [User] SET PFP = NULL WHERE ID = @id",
                conn);

            cmd.Parameters.AddWithValue("@id", userId);

            cmd.ExecuteNonQuery();
        }


    }
}
