using Chess.Web.ViewModels.Chess;
using System.Threading.Tasks;

namespace Chess.Services.Data.Services.Contracts
{
    public interface ICheckService
    {
        Task<bool> IsCheck(BoardViewModel board, string color);
    }
}
