using BLL.Interfaces;   
using GTracker.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Data;

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
                            BoxArt = reader["BoxArt"] != DBNull.Value ? (byte[])reader["BoxArt"] : null,
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

        public List<Game> GetGamesByUserId(int userId)
        {
            var games = new List<Game>();

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                using (var cmd = new SqlCommand(@"
            SELECT 
                g.Id,
                g.Title,
                g.Platform,
                g.ReleaseYear,
                g.Genre,
                g.Status,
                g.Notes,
                g.AddedByUserID,
                g.BoxArt,   

                -- Rating fields
                r.Id AS RatingId,
                r.Stars AS RatingValue

            FROM Game g
            LEFT JOIN Rating r ON g.Id = r.GameId
            WHERE g.AddedByUserID = @uid
        ", conn))
                {
                    cmd.Parameters.AddWithValue("@uid", userId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var statusRaw = reader["Status"]?.ToString() ?? "";
                            var status = Enum.TryParse<GameStatus>(statusRaw, true, out var parsedStatus)
                                ? parsedStatus
                                : GameStatus.CurrentlyPlaying;

                            // Build Game object
                            var game = new Game
                            {
                                Id = (int)reader["Id"],
                                Title = reader["Title"].ToString(),
                                Platform = reader["Platform"].ToString(),
                                releaseYear = (int)reader["ReleaseYear"],
                                Genre = reader["Genre"].ToString(),
                                Status = status,
                                Notes = reader["Notes"].ToString(),
                                AddedByUserID = userId,

                                BoxArt = reader["BoxArt"] != DBNull.Value
                                ? (byte[])reader["BoxArt"]
                                : null
                            };

                            // ⭐ Build Rating object (if found)
                            if (reader["RatingId"] != DBNull.Value)
                            {
                                game.Stars = new Rating
                                {
                                    Id = Convert.ToInt32(reader["RatingId"]),
                                    GameId = game.Id,
                                    Stars = Convert.ToInt32(reader["RatingValue"])
                                };
                            }
                            else
                            {
                                game.Stars = null; // No rating yet
                            }

                            games.Add(game);
                        }
                    }
                }
            }

            return games;
        }


        public Game? GetGameById(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                using (var cmd = new SqlCommand("SELECT * FROM Game WHERE Id = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (!reader.Read()) return null;

                        var statusRaw = reader["Status"]?.ToString() ?? "";
                        var status = Enum.TryParse<GameStatus>(statusRaw, true, out var parsedStatus)
                            ? parsedStatus
                            : GameStatus.CurrentlyPlaying;

                        return new Game
                        {
                            Id = reader["Id"] != DBNull.Value ? Convert.ToInt32(reader["Id"]) : 0,
                            Title = reader["Title"]?.ToString() ?? "(no title)",
                            releaseYear = reader["ReleaseYear"] != DBNull.Value ? Convert.ToInt32(reader["ReleaseYear"]) : 0,
                            Genre = reader["Genre"]?.ToString(),
                            BoxArt = reader["BoxArt"] != DBNull.Value ? (byte[])reader["BoxArt"] : null,
                            Platform = reader["Platform"]?.ToString() ?? "Unknown",
                            Status = status,
                            IsCustom = reader["IsCustom"] != DBNull.Value && (bool)reader["IsCustom"],
                            AddedByUserID = reader["AddedByUserID"] != DBNull.Value
                                ? Convert.ToInt32(reader["AddedByUserID"])
                                : (int?)null,
                            Notes = reader["Notes"]?.ToString()
                        };

                    }
                }
            }
        }

        public Rating? GetRatingById(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                using (var cmd = new SqlCommand("SELECT * FROM Rating WHERE Id = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (!reader.Read()) return null;

                 
                        return new Rating
                        {
                            Id = reader["Id"] != DBNull.Value ? Convert.ToInt32(reader["Id"]) : 0,
                            GameId = reader["GameId"] != DBNull.Value ? Convert.ToInt32(reader["GameId"]) : 0,
                            UserId = reader["UserId"] != DBNull.Value ? Convert.ToInt32(reader["UserId"]) : 0,
                            Stars = reader["Stars"] != DBNull.Value ? Convert.ToInt32(reader["Stars"]) : 0,
                        };
                    }
                }
            }
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

        public void AddRating(Rating rating)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    using (var cmd = new SqlCommand(@"
                    INSERT INTO Rating (UserID, GameID, Stars)
                    VALUES (@userid, @gameid, @stars)", conn))
                    {
                        cmd.Parameters.AddWithValue("@userid", rating.UserId);
                        cmd.Parameters.AddWithValue("@gameid", rating.GameId);
                        cmd.Parameters.AddWithValue("@stars", (int)rating.Stars);


                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding rating: " + ex.Message);
            }

        }

        public void UpdateRating(Rating rating)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    using (var cmd = new SqlCommand(@"
                    UPDATE Rating
                    SET UserID = @userid,
                    GameID = @gameid,
                    Stars = @stars
                    WHERE Id = @id", conn))
                    {
                        cmd.Parameters.AddWithValue("@id", rating.Id);
                        cmd.Parameters.AddWithValue("@userid", rating.UserId);
                        cmd.Parameters.AddWithValue("@gameid", rating.GameId);
                        cmd.Parameters.AddWithValue("@stars", (int)rating.Stars);
                       


                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding rating: " + ex.Message);
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
                    cmd.Parameters.Add("@boxArt", SqlDbType.VarBinary).Value = (object)game.BoxArt ?? DBNull.Value;
                    cmd.Parameters.AddWithValue("@platform", (object?)game.Platform ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@status", game.Status.ToString());
                    cmd.Parameters.AddWithValue("@isCustom", game.IsCustom);
                    cmd.Parameters.AddWithValue("@addedBy", (object?)game.AddedByUserID ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@notes", (object?)game.Notes ?? DBNull.Value); 

                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void UpdateGame(Game game)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                using (var cmd = new SqlCommand(@"
                    UPDATE Game
                    SET Title = @title,
                        ReleaseYear = @year,
                        Genre = @genre,
                        BoxArt = @boxArt,
                        Platform = @platform,
                        Status = @status,
                        IsCustom = @isCustom,
                        AddedByUserID = @addedBy,
                        Notes = @notes
                    WHERE Id = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", game.Id);
                    cmd.Parameters.AddWithValue("@title", game.Title);
                    cmd.Parameters.AddWithValue("@year", game.releaseYear);
                    cmd.Parameters.AddWithValue("@genre", (object?)game.Genre ?? DBNull.Value);
                    cmd.Parameters.Add("@boxArt", SqlDbType.VarBinary).Value = (object)game.BoxArt ?? DBNull.Value;
                    cmd.Parameters.AddWithValue("@platform", (object?)game.Platform ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@status", game.Status.ToString());
                    cmd.Parameters.AddWithValue("@isCustom", game.IsCustom);
                    cmd.Parameters.AddWithValue("@addedBy", (object?)game.AddedByUserID ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@notes", (object?)game.Notes ?? DBNull.Value);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        public List<Game> SearchGames(int userId, string? title, int? releaseYear, string? genre, string? platform, GameStatus? status)
        {
            var games = new List<Game>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var query = "SELECT * FROM Game WHERE AddedByUserID = @uid";

                if (!string.IsNullOrWhiteSpace(title))
                    query += " AND Title LIKE @title";

                if (releaseYear.HasValue)
                    query += " AND ReleaseYear = @releaseYear";

                if (!string.IsNullOrWhiteSpace(genre))
                    query += " AND Genre LIKE @genre";

                if (!string.IsNullOrWhiteSpace(platform))
                    query += " AND Platform LIKE @platform";

                if (status.HasValue)
                    query += " AND Status = @status";

                using (var cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@uid", userId);

                    if (!string.IsNullOrWhiteSpace(title))
                        cmd.Parameters.AddWithValue("@title", $"%{title}%");

                    if (releaseYear.HasValue)
                        cmd.Parameters.AddWithValue("@releaseYear", releaseYear.Value);

                    if (!string.IsNullOrWhiteSpace(genre))
                        cmd.Parameters.AddWithValue("@genre", $"%{genre}%");

                    if (!string.IsNullOrWhiteSpace(platform))
                        cmd.Parameters.AddWithValue("@platform", $"%{platform}%");

                    if (status.HasValue)
                        cmd.Parameters.AddWithValue("@status", status.Value.ToString());

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var statusRaw = reader["Status"]?.ToString() ?? "";
                            var parsedStatus = Enum.TryParse<GameStatus>(statusRaw, true, out var gameStatus)
                                ? gameStatus : GameStatus.CurrentlyPlaying;

                            games.Add(new Game
                            {
                                Id = (int)reader["Id"],
                                Title = reader["Title"].ToString(),
                                Genre = reader["Genre"].ToString(),
                                Platform = reader["Platform"].ToString(),
                                releaseYear = (int)reader["ReleaseYear"],
                                Status = parsedStatus
                            });
                        }

                    }
                }
            }

            return games;
        }
    }
}
