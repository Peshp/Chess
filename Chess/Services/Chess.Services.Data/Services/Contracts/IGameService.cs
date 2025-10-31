namespace Chess.Services.Data.Services.Contracts
{
    using System;
    using System.Threading.Tasks;

    using Chess.Web.ViewModels.Chess;

    public interface IGameService
    {
        Task<BoardViewModel> GetBoard(ClockViewModel model, string userId);

       void SaveBoard(BoardViewModel board, string userId);

        Task AddtoMoveHistory(BoardViewModel board, int pieceId, double toX, double toY);
    }
}
