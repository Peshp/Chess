namespace Chess.Domain.Entities
{
    using System.ComponentModel.DataAnnotations;

    using static Common.Validations.Board;

    public class Board
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(ImageMaxLength)]
        public string Image { get; set; } = string.Empty;
    }
}
