namespace Chess.Services.Data.Models.Engine
{
    using System;
    using System.Linq;
    using Chess.Web.ViewModels.Chess;

    /// <summary>
    /// Represents the Queen chess piece and its movement validation logic.
    /// </summary>
    public class Queen : IMoveValidator
    {
        /// <summary>
        /// Validates whether the move for the Queen is valid based on its movement rules.
        /// </summary>
        /// <param name="piece">The Queen piece to be moved.</param>
        /// <param name="toX">The target X-coordinate of the move.</param>
        /// <param name="toY">The target Y-coordinate of the move.</param>
        /// <param name="board">The current state of the chessboard.</param>
        /// <returns>True if the move is valid; otherwise, false.</returns>
        public bool IsValidMove(FigureViewModel piece, double toX, double toY, BoardViewModel board)
        {
            bool isDiagonal = Math.Abs(piece.PositionX - toX) == Math.Abs(piece.PositionY - toY);
            bool isStraight = piece.PositionX == toX || piece.PositionY == toY;
            if (!isStraight && !isDiagonal)
            {
                return false;
            }

            if (!MoveValidationHelper.IsPathClear(piece.PositionX, piece.PositionY, toX, toY, board))
            {
                return false;
            }

            return MoveValidationHelper.IsValidTarget(piece, toX, toY, board);
        }
    }
}
