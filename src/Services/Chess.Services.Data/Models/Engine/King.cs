namespace Chess.Services.Data.Models.Engine
{
    using System;
    using System.Linq;

    using Chess.Web.ViewModels.Chess;

    /// <summary>
    /// Represents the King piece and its movement validation logic.
    /// </summary>
    public class King : IMoveValidator
    {
        /// <summary>
        /// Determines whether the move is a castle attempt.
        /// </summary>
        /// <param name="king">The king piece attempting the move.</param>
        /// <param name="toX">The target X-coordinate.</param>
        /// <param name="toY">The target Y-coordinate.</param>
        /// <returns>True if the move is a castle attempt; otherwise, false.</returns>
        public bool IsCastleAttempt(FigureViewModel king, double toX, double toY)
        {
            return Math.Abs(king.PositionY - toY) == 0 && Math.Abs(king.PositionX - toX) == 25;
        }

        /// <summary>
        /// Determines whether the king can perform a castle move.
        /// </summary>
        /// <param name="king">The king piece attempting the castle.</param>
        /// <param name="board">The current state of the chessboard.</param>
        /// <param name="toX">The target X-coordinate.</param>
        /// <param name="toY">The target Y-coordinate.</param>
        /// <returns>True if the castle move is valid; otherwise, false.</returns>
        public bool CanCastle(FigureViewModel king, BoardViewModel board, double toX, double toY)
        {
            if (king.IsMoved)
            {
                return false;
            }

            double direction = toX > king.PositionX ? 1 : -1;
            double rookX = direction == 1 ? 87.5 : 0;
            double rookY = king.PositionY;

            var rook = board.Figures.FirstOrDefault(f =>
                f.PositionX == rookX && f.PositionY == rookY && f.Color == king.Color && f.Name == "Rook");
            if (rook == null || rook.IsMoved)
            {
                return false;
            }

            double step = 12.5 * direction;
            double x = king.PositionX + step;

            while (x != rookX)
            {
                if (board.Figures.Any(f => f.PositionX == x && f.PositionY == king.PositionY))
                {
                    return false;
                }

                x += step;
            }

            return true;
        }

        /// <summary>
        /// Validates whether the specified move is valid for the king piece.
        /// </summary>
        /// <param name="piece">The king piece attempting the move.</param>
        /// <param name="toX">The target X-coordinate.</param>
        /// <param name="toY">The target Y-coordinate.</param>
        /// <param name="board">The current state of the chessboard.</param>
        /// <returns>True if the move is valid; otherwise, false.</returns>
        public bool IsValidMove(FigureViewModel piece, double toX, double toY, BoardViewModel board)
        {
            double dx = Math.Abs(piece.PositionX - toX);
            double dy = Math.Abs(piece.PositionY - toY);
            bool isOneStep = (dx <= 12.5) && (dy <= 12.5) && (dx + dy != 0);
            bool isStraight = (dx == 12.5 && dy == 0) || (dx == 0 && dy == 12.5);
            bool isDiagonal = dx == 12.5 && dy == 12.5;

            if (isOneStep && (isStraight || isDiagonal))
            {
                return MoveValidationHelper.IsValidTarget(piece, toX, toY, board);
            }

            if (this.IsCastleAttempt(piece, toX, toY))
            {
                return this.CanCastle(piece, board, toX, toY);
            }

            return false;
        }
    }
}
