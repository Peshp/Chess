namespace Chess.Application.ChessEngine.Validators;

using Presentation.ViewModels.Web;

public class KnightMoveValidator : IMoveValidator
{
    public bool IsValidMove(FigureViewModel piece, double toX, double toY, BoardViewModel board)
    {
        double dx = Math.Abs(piece.PositionX - toX);
        double dy = Math.Abs(piece.PositionY - toY);
        bool isKnightMove = (dx == 25 && dy == 12.5) || (dx == 12.5 && dy == 25);
        if (!isKnightMove)
            return false;
        var target = board.Figures.FirstOrDefault(f => f.PositionX == toX && f.PositionY == toY);
        return target == null || target.Color != piece.Color;
    }
}
