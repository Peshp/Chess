namespace Chess.Services.Data.Models.Engine
{
    using System;
    using System.Linq;

    using Chess.Web.ViewModels.Chess;

    /// <summary>
    /// Represents a Knight chess piece and validates its moves.
    /// </summary>
    public class Knight : IMoveValidator
    {
        /// <summary>
        /// Validates whether the move of the Knight is valid based on its movement rules.
        /// </summary>
        /// <param name="piece">The Knight piece to be moved.</param>
        /// <param name="toX">The target X-coordinate of the move.</param>
        /// <param name="toY">The target Y-coordinate of the move.</param>
        /// <param name="board">The current state of the chessboard.</param>
        /// <returns>True if the move is valid; otherwise, false.</returns>
        public bool IsValidMove(FigureViewModel piece, double toX, double toY, BoardViewModel board)
        {
            double dx = Math.Abs(piece.PositionX - toX);
            double dy = Math.Abs(piece.PositionY - toY);
            bool isKnightMove = (dx == 25 && dy == 12.5) || (dx == 12.5 && dy == 25);
            if (!isKnightMove)
            {
                return false;
            }

            return MoveValidationHelper.IsValidTarget(piece, toX, toY, board);
        }
    }
}
