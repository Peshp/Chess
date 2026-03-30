namespace Chess.Services.Validations.Engine;

using System;
using System.Linq;

using Chess.Web.ViewModels.Chess;

public class King : IMoveValidator
{
    public bool IsCastleAttempt(FigureViewModel king, double toX, double toY)
    {
        return Math.Abs(king.PositionY - toY) == 0 && Math.Abs(king.PositionX - toX) == 25;
    }

    public async Task<bool> CanCastle(FigureViewModel king, BoardViewModel board, double toX, double toY)
    {
        if (king.IsMoved)
        {
            return false;
        }

        double direction = toX > king.PositionX ? 1 : -1;
        double rookX = direction == 1 ? 87.5 : 0;
        double rookY = king.PositionY;

        var rook = board.Figures.FirstOrDefault(f =>
            f.PositionX == rookX && f.PositionY == rookY && f.Color == king.Color && f.Name == "Rook");
        if (rook == null || rook.IsMoved)
        {
            return false;
        }

        double step = 12.5 * direction;
        double x = king.PositionX + step;

        while (x != rookX)
        {
            if (board.Figures.Any(f => f.PositionX == x && f.PositionY == king.PositionY))
            {
                return false;
            }

            x += step;
        }

        return true;
    }

    public async Task<bool> IsValidMoveAsync(FigureViewModel piece, double toX, double toY, BoardViewModel board)
    {
        double dx = Math.Abs(piece.PositionX - toX);
        double dy = Math.Abs(piece.PositionY - toY);
        bool isOneStep = (dx <= 12.5) && (dy <= 12.5) && (dx + dy != 0);
        bool isStraight = (dx == 12.5 && dy == 0) || (dx == 0 && dy == 12.5);
        bool isDiagonal = dx == 12.5 && dy == 12.5;

        if (isOneStep && (isStraight || isDiagonal))
        {
            var target = board.Figures.FirstOrDefault(f => f.PositionX == toX && f.PositionY == toY);
            return target == null || target.Color != piece.Color;
        }

        if (this.IsCastleAttempt(piece, toX, toY))
        {
            return await this.CanCastle(piece, board, toX, toY);
        }

        return false;
    }

    public void PerformCastleMove(BoardViewModel board, FigureViewModel king, double toX, double toY)
    {
        double direction = toX > king.PositionX ? 1 : -1;
        double rookX = direction == 1 ? 87.5 : 0;
        double rookY = king.PositionY;

        var rook = board.Figures.FirstOrDefault(f =>
            f.PositionX == rookX && f.PositionY == rookY && f.Color == king.Color && f.Name == "Rook");
        double toSquare = direction == 1 ? -12.5 : 12.5;

        king.PositionX = toX;
        king.PositionY = toY;
        king.IsMoved = true;
        if (rook != null)
        {
            rook.PositionX = toX + toSquare;
            rook.IsMoved = true;
        }

        board.CurrentTurn = (board.CurrentTurn == "White") ? "Black" : "White";
    }
}