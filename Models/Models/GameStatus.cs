using System.ComponentModel.DataAnnotations;

namespace GTracker.Models
{
     public enum GameStatus
        {
        Owned,
        Wishlist,
        Finished,
        [Display(Name = "Currently Playing")]
        CurrentlyPlaying
    }
    }