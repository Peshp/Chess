using Chess.Web.ViewModels.Chess;
using System.Threading.Tasks;

namespace Chess.Services.Data.Services.Contracts
{
    public interface IEngineService
    {
        Task<bool> TryMove(BoardViewModel board, int pieceId, double toX, double toY);

        Task<bool> IsCheck(BoardViewModel board, string color);

        Task<FigureViewModel> FindPieceById(BoardViewModel board, int pieceId);
    }
}
