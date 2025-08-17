namespace Chess.Application.interfaces
{
    using Domain.ViewModels.Web;
    using infrastructure.Entities;

    public interface IGameService
    {
        Task<BoardViewModel> GetBoard();

        //Task<List<FigureViewModel>> GetFigures();

        Task<bool> TryMove(BoardViewModel board, int pieceId, double toX, double toY);
    }
}
