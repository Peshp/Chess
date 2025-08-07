namespace Chess.Infrastructure.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class Board
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public List<Figure> Figures { get; set; } = new List<Figure>();
    }
}
