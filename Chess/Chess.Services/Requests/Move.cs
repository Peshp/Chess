namespace Chess.Services.Requests;

/// <summary>
/// Represents a move made by a chess piece.
/// </summary>
public class Move
{
    private double toX;
    private double toY;

    public Move(double toX, double toY)
    {
        this.toX = toX * 12.5;
        this.toY = toY * 12.5;
    }

    public int PieceId { get; set; }

    public bool isGameOver { get; set; }

    public double ToX => toX;

    public double ToY => toY;
}
