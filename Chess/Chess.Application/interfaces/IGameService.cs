namespace Chess.Application.interfaces;

using Domain.ViewModels.Web;

public interface IGameService
{
    Task<BoardViewModel> GetBoard();

    Task<bool> TryMove(BoardViewModel board, int pieceId, double toX, double toY);
}
