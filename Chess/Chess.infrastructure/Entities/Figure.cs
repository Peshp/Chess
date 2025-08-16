namespace Chess.infrastructure.Entities
{
    using infrastructure.Enums;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Figure
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public FigureType Type { get; set; }

        [Required]
        public FigureColor Color { get; set; }

        public string Image { get; set; } = string.Empty;

        [Required]
        [ForeignKey(nameof(Board))]
        public int BoardId { get; set; }

        public Board Board { get; set; } = null!;

        public double PositionX { get; set; }

        public double PositionY { get; set; }
    }
}
