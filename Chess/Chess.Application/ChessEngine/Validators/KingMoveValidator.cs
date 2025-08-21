namespace Chess.Application.ChessEngine.Validators;

using Chess.Domain.ViewModels.Web;

public class KingMoveValidator : IMoveValidator
{
    public bool IsValidMove(FigureViewModel piece, double toX, double toY, BoardViewModel board)
    {
        double dx = Math.Abs(piece.PositionX - toX);
        double dy = Math.Abs(piece.PositionY - toY);

        bool isOneStep = (dx <= 12.5) && (dy <= 12.5) && (dx + dy != 0);
        bool isStraight = dx == 12.5 && dy == 0 || dx == 0 && dy == 12.5;
        bool isDiagonal = dx == 12.5 && dy == 12.5;

        if (!isOneStep || (!isStraight && !isDiagonal))
            return false;

        var target = board.Figures.FirstOrDefault(f => f.PositionX == toX && f.PositionY == toY);
        if (target == null)
            return true;

        return target.Color != piece.Color; 
    }
}
