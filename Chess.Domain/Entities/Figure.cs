namespace Chess.Domain.Entities
{
    using System.ComponentModel.DataAnnotations;

    using Enums;
    using static Common.Validations.Figure;

    public class Figure
    {
        public Figure()
        {
            MoveHistory = new List<string>();
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
        [MaxLength(ColorMaxLength)]
        public string Color { get; set; } = string.Empty;

        [Required]
        [MaxLength(FigureImageMaxLength)]
        public string FigureImage { get; set; } = string.Empty;

        [Required]
        [MaxLength(CurrentPositionMaxLength)]
        public string CurrentPosition { get; set; } = string.Empty;

        public ICollection<string> MoveHistory { get; set; } 
    }
}
