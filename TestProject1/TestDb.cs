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
}

