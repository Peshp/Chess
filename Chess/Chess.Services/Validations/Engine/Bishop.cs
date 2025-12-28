namespace Chess.Services.Validations.Engine;

using System;
using System.Linq;

using Chess.Web.ViewModels.Chess;

public class Bishop : BaseMoveValidator
{
    public override bool IsValidMove(FigureViewModel piece, double toX, double toY, BoardViewModel board)
    {
        if (Math.Abs(Math.Abs(piece.PositionX - toX) - Math.Abs(piece.PositionY - toY)) > 0.1) return false;
        if (IsFriendlyFire(piece, toX, toY, board)) return false;
        return IsPathClear(piece.PositionX, piece.PositionY, toX, toY, board);
    }
}
