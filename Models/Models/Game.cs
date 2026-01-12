using System.ComponentModel.DataAnnotations;

namespace GTracker.Models
{
    public class Game
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; } = "";

        [Required(ErrorMessage = "Release Year is required")]
        public int releaseYear { get; set; }

        [Required(ErrorMessage = "Genre is required")]
        public string? Genre { get; set; }

        public byte[] BoxArt { get; set; }

        public byte[] Screenshot { get; set; }

        [Required(ErrorMessage = "Platform is required")]
        public string Platform { get; set; } = "Unknown";

        public GameStatus Status { get; set; } = GameStatus.CurrentlyPlaying;

        public bool IsCustom { get; set; } = false;

        public int? AddedByUserID { get; set; }

        public User? AddedByUser { get; set; }

        public string? Notes { get; set; }

        public Rating Stars { get; set; }

    }

}

