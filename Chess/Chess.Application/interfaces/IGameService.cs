namespace Chess.Application.interfaces;

using Presentation.ViewModels.Web;

public interface IGameService
{
    Task<BoardViewModel> GetBoard();

    Task<bool> TryMove(BoardViewModel board, int pieceId, double toX, double toY);

    Task<bool> IsCheck(BoardViewModel board, string color);
}
