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

        public async Task<bool> IsCheck(BoardViewModel board, string color)
        {
            var king = board.Figures.FirstOrDefault(f => f.Name == "King" && f.Color == color);

            string opponentColor = (color == "White") ? "Black" : "White";

            foreach (var opponentPiece in board.Figures.Where(f => f.Color == opponentColor))
            {
                var validator = moveValidators[opponentPiece.Name];
                if (validator.IsValidMove(opponentPiece, king.PositionX, king.PositionY, board))
                {
                    return true;
                }
            }

            return false;
        }

        public Task<bool> IsLegalMove(BoardViewModel board, FigureViewModel piece, double toX, double toY)
        {
            var origX = piece.PositionX; var origY = piece.PositionY;
            var captured = board.Figures.FirstOrDefault(f => Math.Abs(f.PositionX - toX) < 0.1 && Math.Abs(f.PositionY - toY) < 0.1);
            if (captured != null) board.Figures.Remove(captured);
            piece.PositionX = toX; piece.PositionY = toY;
            bool inCheck = IsCheck(board, piece.Color).Result;
            piece.PositionX = origX; piece.PositionY = origY;
            if (captured != null) board.Figures.Add(captured);
            return Task.FromResult(!inCheck);
        }

        public async Task<bool> HasLegalMoveToEscapeCheck(BoardViewModel board, string color)
        {
            var own = board.Figures.Where(f => f.Color == color).ToList();
            foreach (var piece in own)
            {
                foreach (var move in GetMoves(board, piece))
                    if (await IsLegalMove(board, piece, move.x, move.y)) return true;
            }
            return false;
        }

        private IEnumerable<(double x, double y)> GetMoves(BoardViewModel board, FigureViewModel piece)
        {
            for (double x = 0; x <= 87.5; x += 12.5)
                for (double y = 0; y <= 87.5; y += 12.5)
                    if ((piece.PositionX != x || piece.PositionY != y) &&
                        moveValidators[piece.Name].IsValidMove(piece, x, y, board))
                        yield return (x, y);
        }
    }
}
