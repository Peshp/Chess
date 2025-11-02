using Chess.Data.Models.Enums;

namespace Chess.Data.Models
{
    public class BoardFigures
    {
        public int Id { get; set; }

        public FigureType Type { get; set; }

        public FigureColor Color { get; set; }

        public string Image { get; set; }

        public double PositionX { get; set; }

        public double PositionY { get; set; }

        public int BoardId { get; set; }

        public UserBoard Board { get; set; }
    }
}
