namespace Chess.Services.Data.Models.Engine
{
    using System;
    using System.Linq;
    using Chess.Web.ViewModels.Chess;

    /// <summary>
    /// Provides shared helper methods for chess move validation.
    /// </summary>
    public static class MoveValidationHelper
    {
        /// <summary>
        /// Checks if the path between two positions is clear of other pieces.
        /// Supports straight (horizontal/vertical) and diagonal paths.
        /// </summary>
        /// <param name="fromX">The starting X-coordinate.</param>
        /// <param name="fromY">The starting Y-coordinate.</param>
        /// <param name="toX">The target X-coordinate.</param>
        /// <param name="toY">The target Y-coordinate.</param>
        /// <param name="board">The current state of the chessboard.</param>
        /// <returns>True if the path is clear; otherwise, false.</returns>
        public static bool IsPathClear(double fromX, double fromY, double toX, double toY, BoardViewModel board)
        {
            double dx = 0, dy = 0;

            // Calculate direction for straight moves
            if (fromX == toX)
            {
                dy = (toY > fromY) ? 12.5 : -12.5;
            }
            else if (fromY == toY)
            {
                dx = (toX > fromX) ? 12.5 : -12.5;
            }
            // Calculate direction for diagonal moves
            else if (Math.Abs(fromX - toX) == Math.Abs(fromY - toY))
            {
                dx = (toX > fromX) ? 12.5 : -12.5;
                dy = (toY > fromY) ? 12.5 : -12.5;
            }
            else
            {
                return false;
            }

            double x = fromX + dx;
            double y = fromY + dy;

            while (x != toX || y != toY)
            {
                if (board.Figures.Any(f => f.PositionX == x && f.PositionY == y))
                {
                    return false;
                }

                x += dx;
                y += dy;
            }

            return true;
        }

        /// <summary>
        /// Checks if the target position is a valid destination (empty or contains an enemy piece).
        /// </summary>
        /// <param name="piece">The piece attempting the move.</param>
        /// <param name="toX">The target X-coordinate.</param>
        /// <param name="toY">The target Y-coordinate.</param>
        /// <param name="board">The current state of the chessboard.</param>
        /// <returns>True if the target is valid; otherwise, false.</returns>
        public static bool IsValidTarget(FigureViewModel piece, double toX, double toY, BoardViewModel board)
        {
            var target = board.Figures.FirstOrDefault(f => f.PositionX == toX && f.PositionY == toY);
            return target == null || target.Color != piece.Color;
        }

        /// <summary>
        /// Checks if a specific position on the board is empty.
        /// </summary>
        /// <param name="x">The X-coordinate of the position.</param>
        /// <param name="y">The Y-coordinate of the position.</param>
        /// <param name="board">The current state of the chessboard.</param>
        /// <returns>True if the position is empty; otherwise, false.</returns>
        public static bool IsEmptySquare(double x, double y, BoardViewModel board)
        {
            return board.Figures.FirstOrDefault(f => f.PositionX == x && f.PositionY == y) == null;
        }
    }
}
