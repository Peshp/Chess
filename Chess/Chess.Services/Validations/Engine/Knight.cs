namespace Chess.Services.Validations.Engine;

using System;
using System.Linq;

using Chess.Web.ViewModels.Chess;

public class Knight : BaseMoveValidator
{
    public override bool IsValidMove(FigureViewModel piece, double toX, double toY, BoardViewModel board)
    {
        double dx = Math.Abs(piece.PositionX - toX);
        double dy = Math.Abs(piece.PositionY - toY);
        if (!((Math.Abs(dx - 25) < 0.1 && Math.Abs(dy - 12.5) < 0.1) || (Math.Abs(dx - 12.5) < 0.1 && Math.Abs(dy - 25) < 0.1))) return false;
        return !IsFriendlyFire(piece, toX, toY, board);
    }
}
