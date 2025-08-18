namespace Chess.Application;

using Domain.ViewModels.Web;

public class ChessEngine
{
    private BoardViewModel _board;

    public ChessEngine(BoardViewModel board)
    {
        _board = board;
    }

    public async Task<bool> TryMove(int pieceId, double toX, double toY)
    {
        var piece = _board.Figures.FirstOrDefault(f => f.Id == pieceId);

        if (IsValidMove(piece, toX, toY).Result)
        {
            piece.PositionX = toX;
            piece.PositionY = toY;

            return true;
        }

        return false;
    }

    private async Task<bool> IsValidMove(FigureViewModel piece, double toX, double toY)
    {
        // TODO: real chess rules
        bool valid = valid = IsEmpty(piece, toX, toY); 
        if (piece.Name == "Pawn")
            valid = IsValidPawnMove(piece, toX, toY);       

        return valid;
    }

    private bool IsValidPawnMove(FigureViewModel piece, double toX, double toY)
    {
        double direction = piece.Color == "White" ? -12.5 : 12.5;
        double startRow = piece.Color == "White" ? 75 : 12.5;

        // Forward moves
        if (toX == piece.PositionX)
        {
            if (toY == piece.PositionY + direction && IsEmpty(piece, toX, toY))
                return true;
            if (piece.PositionY == startRow && toY == piece.PositionY + 2 * direction && IsEmpty(piece, toX, toY))
                return true;
        }

        // Diagonal captures
        if ((toX == piece.PositionX - 12.5 || toX == piece.PositionX + 12.5) && toY == piece.PositionY + direction)
        {
            var target = FindPiece(piece, toX, toY);
            if (target != null && target.Color != piece.Color)
                return true;
        }

        return false;
    }

    private FigureViewModel? FindPiece(FigureViewModel piece, double toX, double toY)
        => _board.Figures.Find(f => f.PositionX == toX && f.PositionY == toY);

    private bool IsEmpty(FigureViewModel piece, double toX, double toY)
        => FindPiece == null;
}
