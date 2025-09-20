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
            var piece = this.board.Figures.FirstOrDefault(f => f.Id == pieceId);
            if (piece == null) return false;
            if (piece.Color != this.board.CurrentTurn) return false;
            if (!this.moveValidators.TryGetValue(piece.Name, out var validator)) return false;

            if (!await IsValidMove(piece, toX, toY)) return false;

            var target = FindPiece(toX, toY);
            if (target != null && target.Color != piece.Color)
            {
                this.board.CapturedFigures.Add(target);
                this.board.Figures.Remove(target);
            }

            piece.PositionX = toX;
            piece.PositionY = toY;
            piece.IsMoved = true;

            this.board.CurrentTurn = (this.board.CurrentTurn == "White") ? "Black" : "White";
            return true;
        }

        private FigureViewModel? FindPiece(double x, double y)
            => this.board.Figures.FirstOrDefault(f =>
                Math.Abs(f.PositionX - x) < 0.1 && Math.Abs(f.PositionY - y) < 0.1);

        private async Task<bool> IsValidMove(FigureViewModel piece, double toX, double toY)
        {
            if (this.moveValidators.TryGetValue(piece.Name, out var validator))
                return validator.IsValidMove(piece, toX, toY, this.board);
            return false;
        }
    }
}
