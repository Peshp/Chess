using Chess.Services.Data.Models.Engine;
using Chess.Services.Data.Services.Contracts;
using Chess.Web.ViewModels.Chess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Services.Data.Services
{
    public class CastleService : ICastleService
    {
        private readonly Dictionary<string, IMoveValidator> moveValidators;

        public CastleService()
        {
            moveValidators = new Dictionary<string, IMoveValidator>
            {
                { "Pawn", new Pawn() },
                { "Bishop", new Bishop() },
                { "Rook", new Rook() },
                { "Queen", new Queen() },
                { "King", new King() },
                { "Knight", new Knight() },
            };
        }

        public async Task<bool> IsCastleLegal(
            BoardViewModel board,
            FigureViewModel king,
            double toX,
            double toY)
        {
            double direction = toX > king.PositionX ? 1 : -1;
            double step = 12.5 * direction;
            double originalX = king.PositionX;

            for (int i = 0; i <= 2; i++)
            {
                double x = king.PositionX + step * i;
                king.PositionX = originalX;
            }

            return true;
        }

        public void PerformCastleMove(FigureViewModel king, BoardViewModel board, double toX, double toY)
        {
            var kingValidator = new King();
            kingValidator.DoCastleMove(king, board, toX, toY);
        }

        public bool IsCastleAttempt(FigureViewModel king, double toX, double toY)
        {
            return Math.Abs(king.PositionX - toX) == 25 && king.PositionY == toY;
        }
    }
}
