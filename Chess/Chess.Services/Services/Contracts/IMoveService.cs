namespace Chess.Services.Services.Contracts
{
    using System.Threading.Tasks;

    using Chess.Web.ViewModels.Chess;

    public interface IMoveService
    {
        FigureViewModel? FindPiece(BoardViewModel board, double x, double y);

        void CapturePiece(BoardViewModel board, FigureViewModel target);
    }
}
