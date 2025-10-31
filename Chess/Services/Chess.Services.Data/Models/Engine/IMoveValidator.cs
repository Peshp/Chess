namespace Chess.Services.Data.Models.Engine
{
    using Chess.Web.ViewModels.Chess;

    /// <summary>
    /// Provides an interface for validating chess moves.
    /// </summary>
    public interface IMoveValidator
    {
        /// <summary>
        /// Determines whether a move is valid for a given chess piece on the board.
        /// </summary>
        bool IsValidMove(FigureViewModel piece, double toX, double toY, BoardViewModel board);
    }
}
