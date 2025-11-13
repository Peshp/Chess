namespace Chess.Services.Services.Contracts
{
    using System.Threading.Tasks;

    using Chess.Web.ViewModels.Chess;

    public interface ICastleService
    {
        Task<bool> IsCastleLegal(BoardViewModel board, FigureViewModel king, double toX, double toY, ICheckService checkService);

        void PerformCastleMove(BoardViewModel board, FigureViewModel king, double toX, double toY);
    }
}
