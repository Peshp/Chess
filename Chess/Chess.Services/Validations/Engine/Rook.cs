namespace Chess.Services.Validations.Engine;

using Chess.Web.ViewModels.Chess;

using System.Linq;

/// <summary>
/// Represents a Rook chess piece and validates its moves.
/// </summary>
public class Rook : IMoveValidator
{
    public bool IsValidMove(FigureViewModel piece, double toX, double toY, BoardViewModel board)
    {
        if (piece.PositionX != toX && piece.PositionY != toY)
        {
            return false;
        }

        if (!IsPathClear(piece.PositionX, piece.PositionY, toX, toY, board))
        {
            return false;
        }

        var target = board.Figures.FirstOrDefault(f => f.PositionX == toX && f.PositionY == toY);
        return target == null || target.Color != piece.Color;
    }

    private bool IsPathClear(double fromX, double fromY, double toX, double toY, BoardViewModel board)
    {
        double dx = fromX == toX ? 0 : (toX > fromX ? 12.5 : -12.5);
        double dy = fromY == toY ? 0 : (toY > fromY ? 12.5 : -12.5);
        double x = fromX + dx;
        double y = fromY + dy;

        while (x != toX || y != toY)
        {
            if (board.Figures.Any(f => f.PositionX == x && f.PositionY == y))
                return false;
            x += dx;
            y += dy;
        }

        return true;
    }
}
