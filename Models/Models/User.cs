using System.ComponentModel.DataAnnotations;

namespace GTracker.Models
{
    public class User
    {
    public int ID { get; set; }
        [Required] public string Username { get; set; } = null!;
        [Required] public string Email { get; set; } = null!;   

        [Required] public string PasswordHash { get; set; } = null!;

        public List<Game>? AddedGames { get; set; }
    }

}
