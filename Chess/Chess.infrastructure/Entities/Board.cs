namespace Chess.infrastructure.Entities
{
    public class Board
    {
        public int Id { get; set; }

        public string Image { get; set; } = string.Empty;

        public List<Figure> Figures { get; set; } = new List<Figure>();
    }
}
