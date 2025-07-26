namespace Chess.Application.Interfaces
{
    public interface IChessService
    {
        Task Game();

        Figure GetFigureAt(int row, int col);

        bool IsWhiteSquare(int row, int col);
    }
}
