namespace Chess.Services.Data.Models.Engine
{
    using System;
    using System.Linq;
    using Chess.Web.ViewModels.Chess;

    /// <summary>
    /// Represents a Bishop chess piece and provides functionality to validate its moves.
    /// </summary>
    public class Bishop : IMoveValidator
    {
        /// <summary>
        /// Validates whether the move of the Bishop is valid based on its movement rules.
        /// </summary>
        /// <param name="piece">The Bishop piece to be moved.</param>
        /// <param name="toX">The target X-coordinate of the move.</param>
        /// <param name="toY">The target Y-coordinate of the move.</param>
        /// <param name="board">The current state of the chessboard.</param>
        /// <returns>True if the move is valid; otherwise, false.</returns>
        public bool IsValidMove(FigureViewModel piece, double toX, double toY, BoardViewModel board)
        {
            bool isDiagonal = Math.Abs(piece.PositionX - toX) == Math.Abs(piece.PositionY - toY);
            if (!isDiagonal)
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
