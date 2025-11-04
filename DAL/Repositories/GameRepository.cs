using GTracker.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using DAL.Interfaces;

namespace DAL.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly string _connectionString;

        public GameRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Game> GetAllGames()
        {
            var games = new List<Game>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                using (var cmd = new SqlCommand("SELECT * FROM Game", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var statusRaw = reader["Status"]?.ToString() ?? "";
                        var status = Enum.TryParse<GameStatus>(statusRaw, true, out var parsedStatus)
                            ? parsedStatus
                            : GameStatus.CurrentlyPlaying;

                        var game = new Game
                        {
                            Id = reader["Id"] != DBNull.Value ? Convert.ToInt32(reader["Id"]) : 0,
                            Title = reader["Title"]?.ToString() ?? "(no title)",
                            releaseYear = reader["ReleaseYear"] != DBNull.Value ? Convert.ToInt32(reader["ReleaseYear"]) : 0,
                            Genre = reader["Genre"]?.ToString(),
                            boxArtUrl = reader["BoxArt"]?.ToString(),
                            Platform = reader["Platform"]?.ToString() ?? "Unknown",
                            Status = status,
                            IsCustom = reader["IsCustom"] != DBNull.Value && (bool)reader["IsCustom"],
                            AddedByUserID = reader["AddedByUserID"] != DBNull.Value
                                ? Convert.ToInt32(reader["AddedByUserID"])
                                : (int?)null,
                            Notes = reader["Notes"]?.ToString() 
                        };

                        games.Add(game);
                    }
                }
            }

            return games;
        }
        public void DeleteGame(int gameId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                using (var cmd = new SqlCommand("DELETE FROM Game WHERE Id = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", gameId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void AddGame(Game game)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                using (var cmd = new SqlCommand(@"
                    INSERT INTO Game (Title, ReleaseYear, Genre, BoxArt, Platform, Status, IsCustom, AddedByUserID, Notes)
                    VALUES (@title, @year, @genre, @boxArt, @platform, @status, @isCustom, @addedBy, @notes)", conn))
                {
                    cmd.Parameters.AddWithValue("@title", game.Title);
                    cmd.Parameters.AddWithValue("@year", game.releaseYear);
                    cmd.Parameters.AddWithValue("@genre", (object?)game.Genre ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@boxArt", (object?)game.boxArtUrl ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@platform", (object?)game.Platform ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@status", game.Status.ToString());
                    cmd.Parameters.AddWithValue("@isCustom", game.IsCustom);
                    cmd.Parameters.AddWithValue("@addedBy", (object?)game.AddedByUserID ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@notes", (object?)game.Notes ?? DBNull.Value); // 🆕 new field

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
