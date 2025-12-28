namespace Chess.Services.Validations.Engine;

using System.Linq;

using Chess.Web.ViewModels.Chess;

public class Pawn : BaseMoveValidator
{
    public override bool IsValidMove(FigureViewModel piece, double toX, double toY, BoardViewModel board)
    {
        double direction = piece.Color == "White" ? -12.5 : 12.5;
        double startRow = piece.Color == "White" ? 75.0 : 12.5;
        double dx = Math.Abs(piece.PositionX - toX);
        double dy = toY - piece.PositionY;

        if (dx < 0.1 && Math.Abs(dy - direction) < 0.1)
            return !board.Figures.Any(f => Math.Abs(f.PositionX - toX) < 0.1 && Math.Abs(f.PositionY - toY) < 0.1);

        if (dx < 0.1 && Math.Abs(piece.PositionY - startRow) < 0.1 && Math.Abs(dy - (direction * 2)) < 0.1)
            return IsPathClear(piece.PositionX, piece.PositionY, toX, toY, board) &&
                   !board.Figures.Any(f => Math.Abs(f.PositionX - toX) < 0.1 && Math.Abs(f.PositionY - toY) < 0.1);

        if (Math.Abs(dx - 12.5) < 0.1 && Math.Abs(dy - direction) < 0.1)
        {
            var target = board.Figures.FirstOrDefault(f => Math.Abs(f.PositionX - toX) < 0.1 && Math.Abs(f.PositionY - toY) < 0.1);
            return target != null && target.Color != piece.Color;
        }
        return false;
    }
}
