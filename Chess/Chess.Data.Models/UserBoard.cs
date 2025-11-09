namespace Chess.Data.Models;

public class UserBoard
{
    public UserBoard()
    {
        this.Boards = new HashSet<BoardFigures>();
        this.Squares = new HashSet<BoardSquares>();
    }

    public int Id { get; set; }

    public string Image { get; set; }

    public DateTime Date { get; set; }

    public IEnumerable<BoardFigures> Boards { get; set; }

    public IEnumerable<BoardSquares> Squares { get; set; }

    public string? UserId { get; set; }

    public ApplicationUser? User { get; set; }
}
