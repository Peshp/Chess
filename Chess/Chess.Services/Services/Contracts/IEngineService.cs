namespace Chess.Services.Services.Contracts
{
    using System.Threading.Tasks;

    using Chess.Web.ViewModels.Chess;

    public interface IEngineService
    {
        Task<bool> TryMove(BoardViewModel board, int pieceId, double toX, double toY);

        Task<bool> IsCheckmate(BoardViewModel board, string currentColor);
    }
}
