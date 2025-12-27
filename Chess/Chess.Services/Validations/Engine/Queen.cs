namespace Chess.Services.Validations.Engine;

using System;
using System.Linq;

using Chess.Web.ViewModels.Chess;

public class Queen : BaseMoveValidator
{
    public override bool IsValidMove(FigureViewModel piece, double toX, double toY, BoardViewModel board)
    {
        bool isDiagonal = Math.Abs(Math.Abs(piece.PositionX - toX) - Math.Abs(piece.PositionY - toY)) < 0.1;
        bool isStraight = Math.Abs(piece.PositionX - toX) < 0.1 || Math.Abs(piece.PositionY - toY) < 0.1;
        if (!isDiagonal && !isStraight) return false;
        if (IsFriendlyFire(piece, toX, toY, board)) return false;
        return IsPathClear(piece.PositionX, piece.PositionY, toX, toY, board);
    }
}
