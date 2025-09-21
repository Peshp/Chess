using Chess.Services.Data.Models.Engine;
using Chess.Services.Data.Services.Contracts;
using Chess.Web.ViewModels.Chess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chess.Services.Data.Services
{
    public class CheckService : ICheckService
    {
        private readonly Dictionary<string, IMoveValidator> moveValidators;

        public CheckService()
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

        public async Task<bool> IsCheck(BoardViewModel board, string color)
        {
            var king = board.Figures.FirstOrDefault(f => f.Name == "King" && f.Color == color);
            if (king == null) return false;

            var opponentColor = (color == "White") ? "Black" : "White";
            var opponentPieces = board.Figures.Where(f => f.Color == opponentColor);

            foreach (var piece in opponentPieces)
            {
                if (IsValidMove(board, piece, king.PositionX, king.PositionY))
                    return true;
            }

            return false;
        }

        private bool IsValidMove(BoardViewModel board ,FigureViewModel piece, double toX, double toY)
            => moveValidators.TryGetValue(piece.Name, out var validator) && validator.IsValidMove(piece, toX, toY, board);
    }
}
