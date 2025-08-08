namespace Chess.infrastructure.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class Figure
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Color { get; set; } = string.Empty;

        public string Image { get; set; } = string.Empty;

        public int PositionX { get; set; }

        public int PositionY { get; set; }
    }
}
