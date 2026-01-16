using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace GTracker.IntegrationTests;

public static class TestDb
{
    public const string Conn =
        "Server=TOBYY\\SQLEXPRESS01;Database=dbi559954_gametracketest;Trusted_Connection=True;TrustServerCertificate=True;";

    public static void Reset()
    {
        using var conn = new SqlConnection(Conn);
        conn.Open();

        // Order matters because of FK constraints
        using var cmd = new SqlCommand(@"
            DELETE FROM dbo.RunsOn;
            DELETE FROM dbo.OwnedGame;
            DELETE FROM dbo.Rating;
            DELETE FROM dbo.Game;
            DELETE FROM dbo.[User];
        ", conn);

        cmd.ExecuteNonQuery();
    }

    public static int InsertUser(string username = "test", string email = "test@test.com", string password = "pw")
    {
        using var conn = new SqlConnection(Conn);
        conn.Open();

        using var cmd = new SqlCommand(@"
            INSERT INTO dbo.[User] (Username, Email, [Password])
            OUTPUT INSERTED.ID
            VALUES (@u, @e, @p);
        ", conn);

        cmd.Parameters.AddWithValue("@u", username);
        cmd.Parameters.AddWithValue("@e", email);
        cmd.Parameters.AddWithValue("@p", password);

        return (int)cmd.ExecuteScalar();
    }

    public static int InsertGame(int addedByUserId, string title = "Test Game", string platform = "PC")
    {
        using var conn = new SqlConnection(Conn);
        conn.Open();

        using var cmd = new SqlCommand(@"
        INSERT INTO dbo.Game (Title, ReleaseYear, Genre, BoxArt, Screenshot, Platform, Status, IsCustom, AddedByUserID, Notes)
        OUTPUT INSERTED.Id
        VALUES (@title, @year, @genre, NULL, NULL, @platform, @status, @isCustom, @addedBy, NULL);
    ", conn);

        cmd.Parameters.AddWithValue("@title", title);
        cmd.Parameters.AddWithValue("@year", 2024);
        cmd.Parameters.AddWithValue("@genre", DBNull.Value);
        cmd.Parameters.AddWithValue("@platform", platform);
        cmd.Parameters.AddWithValue("@status", "Wishlist");
        cmd.Parameters.AddWithValue("@isCustom", true);
        cmd.Parameters.AddWithValue("@addedBy", addedByUserId);

        return (int)cmd.ExecuteScalar();
    }

}

