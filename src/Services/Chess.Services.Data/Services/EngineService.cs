namespace Chess.Services.Data.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Chess.Services.Data.Models.Engine;
    using Chess.Web.ViewModels.Chess;

    public class EngineService
    {
        private BoardViewModel board;
        private Dictionary<string, IMoveValidator> moveValidators;

        public EngineService(BoardViewModel board)
        {
            this.board = board;
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

        public async Task<bool> TryMove(int pieceId, double toX, double toY)
        {
            var piece = board.Figures.FirstOrDefault(f => f.Id == pieceId);
            if (piece.Color != board.CurrentTurn)
            {
                return false;
            }

            if (!moveValidators.TryGetValue(piece.Name, out var validator))
            {
                return false;
            }

            if (piece.Name == "King" &&
                validator is King kingValidator &&
                kingValidator.IsCastleAttempt(piece, toX, toY))
            {
                if (!kingValidator.CanCastle(piece, board, toX, toY))
                {
                    return false;
                }
                if (!await IsCastleLegal(piece, toX, toY))
                {
                    return false;
                }

                PerformCastleMove(piece, toX, toY);
                SwitchTurn();
                return true;
            }

            if (!await IsValidMove(piece, toX, toY))
            {
                return false;
            }

            if (await IsSelfCheckAfterMove(piece, toX, toY))
            {
                return false;
            }

            var target = FindPiece(toX, toY);
            if (target != null && target.Color != piece.Color)
            {
                board.CapturedFigures.Add(target);
                board.Figures.Remove(target);
            }

            piece.PositionX = toX;
            piece.PositionY = toY;
            piece.IsMoved = true;

            SwitchTurn();
            return true;
        }

        private void SwitchTurn()
        {
            board.CurrentTurn = (board.CurrentTurn == "White") ? "Black" : "White";
        }

        private async Task<bool> IsSelfCheckAfterMove(FigureViewModel piece, double toX, double toY)
        {
            var originalX = piece.PositionX;
            var originalY = piece.PositionY;
            var captured = FindPiece(toX, toY);

            if (captured != null)
            {
                board.Figures.Remove(captured);
            }

            piece.PositionX = toX;
            piece.PositionY = toY;

            bool kingInCheck = await IsCheck(piece.Color);

            piece.PositionX = originalX;
            piece.PositionY = originalY;
            if (captured != null)
            {
                board.CapturedFigures.Add(captured);
            }

            return kingInCheck;
        }

        public async Task<bool> IsCheck(string color)
        {
            var king = board.Figures.FirstOrDefault(f => f.Name == "King" && f.Color == color);
            if (king == null)
            {
                return false;
            }

            string opponentColor = (color == "White") ? "Black" : "White";
            var opponentPieces = board.Figures.Where(f => f.Color == opponentColor);

            foreach (var piece in opponentPieces)
            {
                if (await IsValidMove(piece, king.PositionX, king.PositionY))
                {
                    return true;
                }
            }

            return false;
        }

        private FigureViewModel? FindPiece(double x, double y)
        {
            return board.Figures.FirstOrDefault(f =>
                Math.Abs(f.PositionX - x) < 0.1 &&
                Math.Abs(f.PositionY - y) < 0.1);
        }

        private async Task<bool> IsValidMove(FigureViewModel piece, double toX, double toY)
        {
            if (moveValidators.TryGetValue(piece.Name, out var validator))
            {
                return validator.IsValidMove(piece, toX, toY, board);
            }

            return false;
        }

        private async Task<bool> IsCastleLegal(FigureViewModel king, double toX, double toY)
        {
            double direction = toX > king.PositionX ? 1 : -1;
            double step = 12.5 * direction;

            for (int i = 0; i <= 2; i++)
            {
                double x = king.PositionX + step * i;
                var originalX = king.PositionX;

                king.PositionX = x;
                bool inCheck = await IsCheck(king.Color);

                king.PositionX = originalX;
                if (inCheck)
                {
                    return false;
                }
            }

            return true;
        }

        private void PerformCastleMove(FigureViewModel king, double toX, double toY)
        {
            double direction = toX > king.PositionX ? 1 : -1;
            double rookX = direction == 1 ? 87.5 : 0;
            double rookY = king.PositionY;

            var rook = board.Figures.FirstOrDefault(f =>
                f.PositionX == rookX &&
                f.PositionY == rookY &&
                f.Color == king.Color &&
                f.Name == "Rook");

            double rookTargetX = toX + (direction == 1 ? -12.5 : 12.5);

            king.PositionX = toX;
            king.PositionY = toY;
            king.IsMoved = true;

            if (rook != null)
            {
                rook.PositionX = rookTargetX;
                rook.IsMoved = true;
            }
        }
    }
}
