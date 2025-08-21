namespace Chess.Application.ChessEngine.Validators;

using Domain.ViewModels.Web;

public class PawnMoveValidator : IMoveValidator
{
    public bool IsValidMove(FigureViewModel piece, double toX, double toY, BoardViewModel board)
    {
        double direction = piece.Color == "White" ? -12.5 : 12.5;
        double startRow = piece.Color == "White" ? 75 : 12.5;

        if (piece.PositionX == toX && piece.PositionY + direction == toY)
        {
            if (IsEmptySquare(toX, toY, board))
                return true;
        }

        if (piece.PositionX == toX && piece.PositionY == startRow && piece.PositionY + direction * 2 == toY)
        {
            double intermediateY = piece.PositionY + direction;
            if (IsEmptySquare(toX, intermediateY, board) && IsEmptySquare(toX, toY, board))
                return true;
        }

        if ((toX == piece.PositionX - 12.5 || toX == piece.PositionX + 12.5)
            && toY == piece.PositionY + direction)
        {
            var target = board.Figures.FirstOrDefault(f => f.PositionX == toX && f.PositionY == toY);
            if (target != null && target.Color != piece.Color)
                return true;
        }

        return false;
    }

    private bool IsEmptySquare(double x, double y, BoardViewModel board)
    {
        return board.Figures.All(f => f.PositionX != x || f.PositionY != y);
    }
}
