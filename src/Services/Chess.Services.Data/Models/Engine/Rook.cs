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

            if (!IsPathClear(piece.PositionX, piece.PositionY, toX, toY, board))
            {
                return false;
            }

            var target = board.Figures.FirstOrDefault(f => f.PositionX == toX && f.PositionY == toY);
            return target == null || target.Color != piece.Color;
        }

        /// <summary>
        /// Checks if the path between the starting position and the target position is clear of other pieces.
        /// </summary>
        /// <param name="fromX">The starting X-coordinate of the Rook.</param>
        /// <param name="fromY">The starting Y-coordinate of the Rook.</param>
        /// <param name="toX">The target X-coordinate of the move.</param>
        /// <param name="toY">The target Y-coordinate of the move.</param>
        /// <param name="board">The current state of the chessboard.</param>
        /// <returns>True if the path is clear; otherwise, false.</returns>
        private bool IsPathClear(double fromX, double fromY, double toX, double toY, BoardViewModel board)
        {
            double dx = fromX == toX ? 0 : (toX > fromX ? 12.5 : -12.5);
            double dy = fromY == toY ? 0 : (toY > fromY ? 12.5 : -12.5);
            double x = fromX + dx;
            double y = fromY + dy;

            while (x != toX || y != toY)
            {
                if (board.Figures.Any(f => f.PositionX == x && f.PositionY == y))
                    return false;
                x += dx;
                y += dy;
            }

            return true;
        }
    }
}
