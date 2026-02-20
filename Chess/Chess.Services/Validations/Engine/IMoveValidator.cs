namespace Chess.Services.Validations.Engine;

using Chess.Web.ViewModels.Chess;

public interface IMoveValidator
{
    Task<bool> IsValidMoveAsync(FigureViewModel piece, double toX, double toY, BoardViewModel board);
}
