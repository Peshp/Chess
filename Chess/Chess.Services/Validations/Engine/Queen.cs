namespace Chess.Services.Validations.Engine;

using System;
using System.Linq;

using Chess.Web.ViewModels.Chess;

public class Queen : IMoveValidator
{
    public bool IsValidMove(FigureViewModel piece, double toX, double toY, BoardViewModel board)
    {
        bool isDiagonal = Math.Abs(piece.PositionX - toX) == Math.Abs(piece.PositionY - toY);
        bool isStraight = piece.PositionX == toX || piece.PositionY == toY;
        if (!isStraight && !isDiagonal)
        {
            return false;
        }

        if (!this.IsPathClear(piece.PositionX, piece.PositionY, toX, toY, board))
        {
            return false;
        }

        var target = board.Figures.FirstOrDefault(f => f.PositionX == toX && f.PositionY == toY);
        return target == null || target.Color != piece.Color;
    }

    /// <summary>
    /// Checks if the path between the starting position and the target position is clear of other pieces.
    /// </summary>
    /// <param name="fromX">The starting X-coordinate of the move.</param>
    /// <param name="fromY">The starting Y-coordinate of the move.</param>
    /// <param name="toX">The target X-coordinate of the move.</param>
    /// <param name="toY">The target Y-coordinate of the move.</param>
    /// <param name="board">The current state of the chessboard.</param>
    /// <returns>True if the path is clear; otherwise, false.</returns>
    private bool IsPathClear(double fromX, double fromY, double toX, double toY, BoardViewModel board)
    {
        double dx = 0, dy = 0;
        if (fromX == toX)
        {
            dy = (toY > fromY) ? 12.5 : -12.5;
        }

        else if (fromY == toY)
        {
            dx = (toX > fromX) ? 12.5 : -12.5;
        }
        else if (Math.Abs(fromX - toX) == Math.Abs(fromY - toY))
        {
            dx = (toX > fromX) ? 12.5 : -12.5;
            dy = (toY > fromY) ? 12.5 : -12.5;
        }
        else
        {
            return false;
        }

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
