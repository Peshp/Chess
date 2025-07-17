namespace Chess.Domain.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class Board
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Image { get; set; } = string.Empty;
    }
}
