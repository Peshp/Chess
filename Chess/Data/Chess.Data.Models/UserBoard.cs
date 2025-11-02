using System.Collections.Generic;

namespace Chess.Data.Models
{
    public class UserBoard
    {
        public UserBoard()
        {
            this.Boards = new HashSet<BoardFigures>();
            this.Squares = new HashSet<BoardSquares>();
        }

        public int Id { get; set; }

        public string Image { get; set; }

        public ICollection<BoardFigures> Boards { get; set; }

        public ICollection<BoardSquares> Squares { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
    }
}
