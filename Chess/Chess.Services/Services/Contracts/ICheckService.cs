namespace Chess.Services.Services.Contracts
{
    using System.Threading.Tasks;

    using Chess.Web.ViewModels.Chess;

    public interface ICheckService
    {
        Task<bool> IsCheck(BoardViewModel board, string color);

        Task<bool> IsSelfCheckAfterMove(BoardViewModel board, FigureViewModel piece, double toX, double toY, IMoveService moveService);
    }
}
