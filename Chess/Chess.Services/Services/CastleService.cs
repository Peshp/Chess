namespace Chess.Services.Services;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Chess.Services.Services.Contracts;
using Chess.Services.Validations.Engine;
using Chess.Web.ViewModels.Chess;

public class CastleService : ICastleService
{
    public CastleService()
    {
    }

    public async Task<bool> IsCastleLegal(BoardViewModel board, FigureViewModel king, double toX, double toY, ICheckService checkService)
    {
        double direction = toX > king.PositionX ? 1 : -1;
        double step = 12.5 * direction;

        for (int i = 0; i <= 2; i++)
        {
            double x = king.PositionX + step * i;
            var originalX = king.PositionX;

            king.PositionX = x;
            bool inCheck = await checkService.IsCheck(board, king.Color);

            king.PositionX = originalX;
            if (inCheck) return false;
        }

        return true;
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
    }
}