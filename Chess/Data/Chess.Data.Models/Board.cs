namespace Chess.Data.Models
{
    using System;
    using System.Collections.Generic;

    public class Board
    {
        public Board()
        {
            this.Figures = new HashSet<Figure>();
            this.Movehistory = new HashSet<Square>();
        }

        public int Id { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public string Image { get; set; }

        public ICollection<Figure> Figures { get; set; }

        public ICollection<Square> Movehistory { get; set; }
    }
}
