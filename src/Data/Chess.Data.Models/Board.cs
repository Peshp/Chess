namespace Chess.Data.Models
{
    using System.Collections.Generic;

    public class Board
    {
        public Board()
        {
            this.Figures = new HashSet<Figure>();
        }

        public int Id { get; set; }

        public string Image { get; set; } = string.Empty;

        public ICollection<Figure> Figures { get; set; }
    }
}
