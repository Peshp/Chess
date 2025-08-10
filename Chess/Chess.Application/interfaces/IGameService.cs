namespace Chess.Application.interfaces
{
    using Domain.ViewModels.Web;
    using infrastructure.Entities;

    public interface IGameService
    {
        Task<BoardViewModel> GetBoard();

        Task<List<Figure>> GetFigures();

        Task<bool> TryMove(int pieceId, int toX, int toY);
    }
}
