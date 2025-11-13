namespace Chess.Services;

/// <summary>
/// Represents a move made by a chess piece.
/// </summary>
public class Move
{
    public int PieceId { get; set; }

    public double ToX { get; set; }

    public double ToY { get; set; }
}
