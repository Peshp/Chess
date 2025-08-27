namespace Chess.Application.ChessEngine.Validators;

using Presentation.ViewModels.Web;

public interface IMoveValidator
{
    bool IsValidMove(FigureViewModel piece, double toX, double toY, BoardViewModel board);
}
