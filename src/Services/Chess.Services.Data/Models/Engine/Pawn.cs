namespace Chess.Services.Data.Models.Engine
{
    using System.Linq;

    using Chess.Web.ViewModels.Chess;

    /// <summary>
    /// Represents a Pawn piece and its move validation logic.
    /// </summary>
    public class Pawn : IMoveValidator
    {
        /// <summary>
        /// Validates whether a move for the pawn is valid based on its current position, target position, and the state of the board.
        /// </summary>
        /// <param name="piece">The pawn piece to validate the move for.</param>
        /// <param name="toX">The target X-coordinate of the move.</param>
        /// <param name="toY">The target Y-coordinate of the move.</param>
        /// <param name="board">The current state of the chessboard.</param>
        /// <returns>True if the move is valid; otherwise, false.</returns>
        public bool IsValidMove(FigureViewModel piece, double toX, double toY, BoardViewModel board)
        {
            double direction = piece.Color == "White" ? -12.5 : 12.5;
            double startRow = piece.Color == "White" ? 75 : 12.5;

            if (piece.PositionX == toX && piece.PositionY + direction == toY)
            {
                if (IsEmptySquare(toX, toY, board))
                {
                    return true;
                }
            }

            if (piece.PositionX == toX && piece.PositionY == startRow && piece.PositionY + direction * 2 == toY)
            {
                double intermediateY = piece.PositionY + direction;
                if (this.IsEmptySquare(toX, intermediateY, board) && IsEmptySquare(toX, toY, board))
                {
                    return true;
                }
            }

            if ((toX == piece.PositionX - 12.5 || toX == piece.PositionX + 12.5)
                && toY == piece.PositionY + direction)
            {
                var target = board.Figures.FirstOrDefault(f => f.PositionX == toX && f.PositionY == toY);
                if (target != null && target.Color != piece.Color)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if a specific square on the board is empty.
        /// </summary>
        /// <param name="x">The X-coordinate of the square.</param>
        /// <param name="y">The Y-coordinate of the square.</param>
        /// <param name="board">The current state of the chessboard.</param>
        /// <returns>True if the square is empty; otherwise, false.</returns>
        private bool IsEmptySquare(double x, double y, BoardViewModel board)
        {
            foreach (var f in board.Figures)
            {
                if (f.PositionX == x && f.PositionY == y)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
