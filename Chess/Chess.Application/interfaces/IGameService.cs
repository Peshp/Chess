namespace Chess.Application.interfaces
{
    using Domain.ViewModels.Web;
    using infrastructure.Entities;

    public interface IGameService
    {
        Task<BoardViewModel> GetBoard();

        //Task<List<FigureViewModel>> GetFigures();

        Task<bool> TryMove(int pieceId, double toX, double toY, BoardViewModel board);
    }
}
