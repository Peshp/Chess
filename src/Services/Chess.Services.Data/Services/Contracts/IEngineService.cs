using Chess.Web.ViewModels.Chess;
using System.Threading.Tasks;

namespace Chess.Services.Data.Services.Contracts
{
    public interface IEngineService
    {
        Task<bool> TryMove(BoardViewModel board ,int pieceId, double toX, double toY);

        Task<FigureViewModel?> FindPiece(BoardViewModel board, double x, double y);

        Task<bool> IsValidMove(BoardViewModel board, FigureViewModel piece, double toX, double toY);

        Task<FigureViewModel> FindPieceById(BoardViewModel board, int Id);
    }
}
