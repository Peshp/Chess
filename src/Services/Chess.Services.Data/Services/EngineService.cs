namespace Chess.Services.Data.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Chess.Services.Data.Models.Engine;
    using Chess.Services.Data.Services.Contracts;
    using Chess.Web.ViewModels.Chess;

    public class EngineService : IEngineService
    {
        private Dictionary<string, IMoveValidator> moveValidators;

        public EngineService()
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

        public async Task<bool> TryMove(BoardViewModel board, int pieceId, double toX, double toY)
        {
            var piece = board.Figures.FirstOrDefault(f => f.Id == pieceId);
            if (piece == null) return false;
            if (piece.Color != board.CurrentTurn) return false;
            if (!this.moveValidators.TryGetValue(piece.Name, out var validator)) return false;

            if (!await IsValidMove(board, piece, toX, toY)) return false;
            if (await IsSelfCheckAfterMove(board, piece, toX, toY)) return false;

            var target = FindPiece(board, toX, toY);
            if (target != null && target.Color != piece.Color)
            {
                board.CapturedFigures.Add(target);
                board.Figures.Remove(target);
            }

            piece.PositionX = toX;
            piece.PositionY = toY;
            piece.IsMoved = true;

            board.CurrentTurn = (board.CurrentTurn == "White") ? "Black" : "White";
            return true;
        }

        private async Task<bool> IsSelfCheckAfterMove(BoardViewModel board, FigureViewModel piece, double toX, double toY)
        {
            var originalX = piece.PositionX;
            var originalY = piece.PositionY;
            var captured = FindPiece(board, toX, toY);

            if (captured != null) board.Figures.Remove(captured);
            piece.PositionX = toX;
            piece.PositionY = toY;

            bool leavesKingInCheck = await IsCheck(board, piece.Color);

            piece.PositionX = originalX;
            piece.PositionY = originalY;
            if (captured != null) board.Figures.Add(captured);

            return leavesKingInCheck;
        }

        public async Task<bool> IsCheck(BoardViewModel board, string color)
        {
            var king = board.Figures.FirstOrDefault(f => f.Name == "King" && f.Color == color);
            if (king == null) return false;

            var opponentColor = (color == "White") ? "Black" : "White";
            var opponentPieces = board.Figures.Where(f => f.Color == opponentColor);

            foreach (var piece in opponentPieces)
            {
                if (await IsValidMove(board, piece, king.PositionX, king.PositionY))
                    return true;
            }

            return false;
        }

        public async Task<FigureViewModel> FindPieceById(BoardViewModel board, int pieceId)
            => board.Figures.FirstOrDefault(f => f.Id == pieceId);

        private FigureViewModel? FindPiece(BoardViewModel board, double x, double y)
            => board.Figures.FirstOrDefault(f =>
                Math.Abs(f.PositionX - x) < 0.1 && Math.Abs(f.PositionY - y) < 0.1);

        private async Task<bool> IsValidMove(BoardViewModel board, FigureViewModel piece, double toX, double toY)
        {
            if (this.moveValidators.TryGetValue(piece.Name, out var validator))
                return validator.IsValidMove(piece, toX, toY, board);
            return false;
        }
    }
}
