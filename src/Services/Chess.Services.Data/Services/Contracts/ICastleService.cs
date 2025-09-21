using Chess.Web.ViewModels.Chess;
using System.Threading.Tasks;

namespace Chess.Services.Data.Services.Contracts
{
    public interface ICastleService
    {
        Task<bool> Castle(BoardViewModel board, FigureViewModel king, double toX, double toY);
    }
}
