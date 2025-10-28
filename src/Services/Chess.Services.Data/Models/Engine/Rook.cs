namespace Chess.Services.Data.Models.Engine
{
    using Chess.Web.ViewModels.Chess;
    using System.Linq;

    /// <summary>
    /// Represents a Rook chess piece and validates its moves.
    /// </summary>
    public class Rook : IMoveValidator
    {
        /// <summary>
        /// Validates whether the move for the Rook is valid based on its movement rules.
        /// </summary>
        /// <param name="piece">The Rook piece to be moved.</param>
        /// <param name="toX">The target X-coordinate of the move.</param>
        /// <param name="toY">The target Y-coordinate of the move.</param>
        /// <param name="board">The current state of the chessboard.</param>
        /// <returns>True if the move is valid; otherwise, false.</returns>
        public bool IsValidMove(FigureViewModel piece, double toX, double toY, BoardViewModel board)
        {
            if (piece.PositionX != toX && piece.PositionY != toY)
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
