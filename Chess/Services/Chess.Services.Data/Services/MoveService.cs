namespace Chess.Services.Data.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Chess.Services.Data.Models.Engine;
    using Chess.Services.Data.Services.Contracts;
    using Chess.Web.ViewModels.Chess;

    public class MoveService : IMoveService
    {
        private readonly Dictionary<string, IMoveValidator> moveValidators;

        public MoveService()
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

        public async Task<bool> IsValidMove(BoardViewModel board, FigureViewModel piece, double toX, double toY)
        {
            if (this.moveValidators.TryGetValue(piece.Name, out var validator))
                return validator.IsValidMove(piece, toX, toY, board);
            return false;
        }

        public FigureViewModel? FindPiece(BoardViewModel board, double x, double y)
            => board.Figures.FirstOrDefault(f =>
                Math.Abs(f.PositionX - x) < 0.1 && Math.Abs(f.PositionY - y) < 0.1);

        public void CapturePiece(BoardViewModel board, FigureViewModel target)
        {
            board.CapturedFigures.Add(target);
            board.Figures.Remove(target);
        }
    }
}
