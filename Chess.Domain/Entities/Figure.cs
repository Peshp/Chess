namespace Chess.Domain.Entities
{
    using Enums;
    using System.ComponentModel.DataAnnotations;

    public class Figure
    {
        public Figure()
        {
            MoveHistory = new HashSet<string>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public FigureType Type { get; set; }

        [Required]
        public int Row { get; set; }

        [Required]
        public int Col { get; set; }

        [Required]
        public string Color { get; set; } = string.Empty;

        [Required]
        public string FigureImage { get; set; } = string.Empty;

        [Required]
        public string CurrentPosition { get; set; } = string.Empty;

        public ICollection<string> MoveHistory { get; set; } 
    }
}
