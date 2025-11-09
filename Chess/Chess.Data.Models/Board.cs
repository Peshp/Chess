namespace Chess.Data.Models;

public class Board
{
    public Board()
    {
        this.Figures = new HashSet<Figure>();
        this.Movehistory = new HashSet<Square>();
    }

    public int Id { get; set; }

    public string Image { get; set; }

    public IEnumerable<Figure> Figures { get; set; }

    public IEnumerable<Square> Movehistory { get; set; }
}
