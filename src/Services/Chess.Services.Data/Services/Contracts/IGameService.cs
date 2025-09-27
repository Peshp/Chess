namespace Chess.Services.Data.Services.Contracts
{
    using System.Threading.Tasks;

    using Chess.Web.ViewModels.Chess;

    public interface IGameService
    {
        Task<BoardViewModel> GetBoard();

        //void SaveBoard(BoardViewModel board);

        Task AddtoMoveHistory(BoardViewModel board, int pieceId, double toX, double toY);
    }
}
