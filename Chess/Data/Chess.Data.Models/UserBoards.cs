using System.Collections.Generic;

namespace Chess.Data.Models
{
    public class UserBoards
    {
        public int Board { get; set; }

        public ICollection<Board> Boards { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
    }
}
