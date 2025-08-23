namespace Chess.Application.ChessEngine.Validators;

using Chess.Domain.ViewModels.Web;
using Chess.infrastructure.Entities;

public class KingMoveValidator : IMoveValidator
{
    public bool IsValidMove(FigureViewModel piece, double toX, double toY, BoardViewModel board)
    {
        double directionX = Math.Abs(piece.PositionX - toX);
        double directionY = Math.Abs(piece.PositionY - toY);
       
        bool isOneStep = (directionX <= 12.5) && (directionY <= 12.5) && (directionX + directionY != 0);
        bool isStraight = directionX == 12.5 && directionY == 0 || directionX == 0 && directionY == 12.5;
        bool isDiagonal = directionX == 12.5 && directionY == 12.5;

        if (!isOneStep || (!isStraight && !isDiagonal))
        {
            if (toX - piece.PositionX == 25 || piece.PositionX - toX == 25)
                Castle(piece, board, toX, toY);
            else
                return false;
        }

        var target = board.Figures.FirstOrDefault(f => f.PositionX == toX && f.PositionY == toY);
        if (target == null)
            return true;

        return target.Color != piece.Color; 
    }

    public bool Castle(FigureViewModel king, BoardViewModel board, double toX, double toY)
    {
        if (king.IsMoved)
            return false;

        double direction = toX > king.PositionX ? 1 : -1; 
        double rookX = direction == 1 ? 87.5 : 0; 
        double rookY = king.PositionY; 

        FigureViewModel? rook = board.Figures.FirstOrDefault(f => f.PositionX == rookX 
            && f.PositionY == rookY && f.Color == king.Color);
        if (rook == null || rook.IsMoved)
            return false;

        double step = 12.5 * direction;
        for (double x = king.PositionX + step; x != rookX; x += step)
        {
            if (board.Figures.Any(f => f.PositionX == x && f.PositionY == king.PositionY))
                return false;
        }

        // You should also check if king passes through check or ends in check here!
        // (Not included in this sample; implement in your game engine if needed)

        double toSquare = direction == 1 ? -12.5 : 12.5;
        rook.PositionX = toX + toSquare;

        king.IsMoved = true;
        rook.IsMoved = true;

        return true;
    }
}
