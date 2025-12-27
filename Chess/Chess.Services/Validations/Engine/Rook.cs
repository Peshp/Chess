namespace Chess.Services.Validations.Engine;

using Chess.Web.ViewModels.Chess;

using System.Linq;

public class Rook : BaseMoveValidator
{
    public override bool IsValidMove(FigureViewModel piece, double toX, double toY, BoardViewModel board)
    {
        if (!(Math.Abs(piece.PositionX - toX) < 0.1 || Math.Abs(piece.PositionY - toY) < 0.1)) return false;
        if (IsFriendlyFire(piece, toX, toY, board)) return false;
        return IsPathClear(piece.PositionX, piece.PositionY, toX, toY, board);
    }
}
