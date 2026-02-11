namespace Chess.Services.Helpers;

using Chess.Web.ViewModels.Chess;

public record ChessMove(string? PieceId, double ToX, double ToY);

public static class ParseUciMove
{
    public static ChessMove FromUci(ReadOnlySpan<char> uci, BoardViewModel board)
    {
        const double GridSize = 12.5;

        double fromX = (uci[0] - 'a') * GridSize;
        double fromY = (8 - (int)char.GetNumericValue(uci[1])) * GridSize;

        double targetX = (uci[2] - 'a') * GridSize;
        double targetY = (8 - (int)char.GetNumericValue(uci[3])) * GridSize;

        var piece = board.Figures.FirstOrDefault(f =>
            Math.Abs(f.PositionX - fromX) < 0.1 &&
            Math.Abs(f.PositionY - fromY) < 0.1);

        return new ChessMove(piece?.Id.ToString(), targetX, targetY);
    }
}
