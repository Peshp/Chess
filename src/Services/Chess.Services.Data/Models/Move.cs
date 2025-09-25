namespace Chess.Services.Data.Models
{
    /// <summary>
    /// Represents a move made by a chess piece.
    /// </summary>
    public class Move
    {
        /// <summary>
        /// Gets or sets the identifier of the chess piece making the move.
        /// </summary>
        public int PieceId { get; set; }

        /// <summary>
        /// Gets or sets the X-coordinate of the destination position.
        /// </summary>
        public double ToX { get; set; }

        /// <summary>
        /// Gets or sets the Y-coordinate of the destination position.
        /// </summary>
        public double ToY { get; set; }
    }
}
