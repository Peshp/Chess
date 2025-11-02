namespace Chess.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Chess.Data.Models.Enums;

    public class Figure
    {
        public int Id { get; set; }

        public FigureType Type { get; set; }

        public FigureColor Color { get; set; }

        public string Image { get; set; } = string.Empty;

        public int BoardId { get; set; }

        public Board Board { get; set; } = null!;

        public double PositionX { get; set; }

        public double PositionY { get; set; }
    }
}
