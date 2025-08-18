namespace Chess.infrastructure.Entities;

public class Board
{
    public Board()
    {
        Figures = new HashSet<Figure>();
    }

    public int Id { get; set; }

    public string Image { get; set; } = string.Empty;

    public ICollection<Figure> Figures { get; set; }
}
