namespace Chess.Services.Services.Engine;

using Chess.Web.ViewModels.Chess;

/// <summary>
/// Provides an interface for validating chess moves.
/// </summary>
public interface IMoveValidator
{
    bool IsValidMove(FigureViewModel piece, double toX, double toY, BoardViewModel board);
}
