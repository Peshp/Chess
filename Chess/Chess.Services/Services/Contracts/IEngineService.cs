namespace Chess.Services.Services.Contracts;

using System.Threading.Tasks;

using Chess.Web.ViewModels.Chess;

public interface IEngineService
{
    Task<bool> TryMove(BoardViewModel board, int pieceId, double toX, double toY);

    Task<bool> IsCheckmate(BoardViewModel board, string currentColor, string userId);

    Task<bool> PawnOnEdge(BoardViewModel board, int pieceId);

    Task<bool> IsSelfCheckAfterMove(BoardViewModel board, FigureViewModel piece, double toX, double toY);

    Task<bool> IsCheck(BoardViewModel board, string color);
}
