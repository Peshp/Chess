namespace Chess.Services.Services.Contracts
{
    using System.Threading.Tasks;

    using Chess.Web.ViewModels.Chess;

    public interface IMoveService
    {
        Task<bool> IsValidMove(BoardViewModel board, FigureViewModel piece, double toX, double toY);

        FigureViewModel? FindPiece(BoardViewModel board, double x, double y);

        void CapturePiece(BoardViewModel board, FigureViewModel target);
    }
}
