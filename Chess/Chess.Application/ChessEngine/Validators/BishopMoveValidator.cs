namespace Chess.Application.ChessEngine.Validators;

using Domain.ViewModels.Web;

public class BishopMoveValidator : IMoveValidator
{
    public bool IsValidMove(FigureViewModel piece, double toX, double toY, BoardViewModel board)
    {
        bool isDiagonal = Math.Abs(piece.PositionX - toX) == Math.Abs(piece.PositionY - toY);
        if (!isDiagonal)
            return false;

        if (!IsPathClear(piece.PositionX, piece.PositionY, toX, toY, board))
            return false;

        var target = board.Figures.FirstOrDefault(f => f.PositionX == toX && f.PositionY == toY);
        if (target == null)
            return true; 

        return target.Color != piece.Color; 
    }

    private bool IsPathClear(double fromX, double fromY, double toX, double toY, BoardViewModel board)
    {
        double dx = toX > fromX ? 12.5 : -12.5;
        double dy = toY > fromY ? 12.5 : -12.5;

        double x = fromX + dx;
        double y = fromY + dy;

        while (x != toX && y != toY)
        {
            if (board.Figures.Any(f => f.PositionX == x && f.PositionY == y))
                return false;

            x += dx;
            y += dy;
        }

        return true;
    }
}
