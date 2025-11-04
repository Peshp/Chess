namespace Chess.Services.Services.Engine;

using System;
using System.Linq;

using Chess.Web.ViewModels.Chess;

/// <summary>
/// Represents a Knight chess piece and validates its moves.
/// </summary>
public class Knight : IMoveValidator
{
    public bool IsValidMove(FigureViewModel piece, double toX, double toY, BoardViewModel board)
    {
        double dx = Math.Abs(piece.PositionX - toX);
        double dy = Math.Abs(piece.PositionY - toY);
        bool isKnightMove = (dx == 25 && dy == 12.5) || (dx == 12.5 && dy == 25);
        if (!isKnightMove)
        {
            return false;
        }

        var target = board.Figures.FirstOrDefault(f => f.PositionX == toX && f.PositionY == toY);
        return target == null || target.Color != piece.Color;
    }
}
