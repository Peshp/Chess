using Chess.Services.Data.Models.Engine;
using Chess.Services.Data.Services.Contracts;
using Chess.Web.ViewModels.Chess;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chess.Services.Data.Services
{
    public class CastleService : ICastleService
    {
        private Dictionary<string, IMoveValidator> moveValidators;

        public CastleService()
        {
            this.moveValidators = new Dictionary<string, IMoveValidator>
            {
                { "Pawn", new Pawn() },
                { "Bishop", new Bishop() },
                { "Rook", new Rook() },
                { "Queen", new Queen() },
                { "King", new King() },
                { "Knight", new Knight() },
            };
        }

        public async Task<bool> Castle(BoardViewModel board, int pieceId, double toX, double toY)
        {
            var piece = board.Figures.FirstOrDefault(f => f.Id == pieceId);
            if (!this.moveValidators.TryGetValue(piece.Name, out var validator)) return false;

            if (piece.Name == "King" &&
                validator is King kingValidator &&
                kingValidator.IsCastleAttempt(piece, toX, toY))
            {
                if (!kingValidator.CanCastle(piece, board, toX, toY)) return false;
                if (!await IsCastleLegal(board, piece, toX, toY)) return false;
                PerformCastleMove(board, piece, toX, toY);
                board.CurrentTurn = (board.CurrentTurn == "White") ? "Black" : "White";
                return true;
            }

            return false;
        }

        private void PerformCastleMove(BoardViewModel board, FigureViewModel king, double toX, double toY)
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

        private async Task<bool> IsCastleLegal(BoardViewModel board, FigureViewModel king, double toX, double toY)
        {
            var engine = new EngineService();
            double direction = toX > king.PositionX ? 1 : -1;
            double step = 12.5 * direction;

            for (int i = 0; i <= 2; i++)
            {
                double x = king.PositionX + step * i;
                var originalX = king.PositionX;

                king.PositionX = x;
                bool inCheck = await engine.IsCheck(board, king.Color);

                king.PositionX = originalX;
                if (inCheck) return false;
            }

            return true;
        }
    }
}
