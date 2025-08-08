namespace Chess.Application.interfaces
{
    using Chess.Domain.ViewModels.Web;

    public interface IGameService
    {
        Task<BoardViewModel> GetBoard();

        Task<List<FigureViewModel>> GetFigures();
    }
}
