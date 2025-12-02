using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTracker.Models
{
    public class Rating
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int GameId { get; set; }

        public int Stars { get; set; }
    }
}
