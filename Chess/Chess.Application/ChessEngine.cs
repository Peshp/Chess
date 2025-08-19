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
        FigureViewModel piece = _board.Figures.FirstOrDefault(f => f.Id == pieceId);

        if (await IsValidMove(piece, toX, toY))
        {
            var target = FindPiece(toX, toY);
            if (target != null && target.Color != piece.Color)
            {
                _board.Figures.Remove(target);
                _board.CapturedFigures.Add(target);
            }

            piece.PositionX = toX;
            piece.PositionY = toY;

            return true;
        }

        return false;
    }

    private async Task<bool> IsValidMove(FigureViewModel piece, double toX, double toY)
    {
        // TODO: real chess rules
        bool valid = false; 
        if (piece.Name == "Pawn")
            valid = IsValidPawnMove(piece, toX, toY);       

        return valid;
    }

    private bool IsValidPawnMove(FigureViewModel piece, double toX, double toY)
    {
        double direction = piece.Color == "White" ? -12.5 : 12.5;
        double startRow = piece.Color == "White" ? 75 : 12.5;

        if (FindPiece(toX, toY) == null && piece.PositionX == toX)
        {
            if (piece.PositionY + direction == toY)
                return true;

            if (piece.PositionY == startRow &&
                piece.PositionY + direction * 2 == toY)
                return true;
        }

        if ((toX == piece.PositionX - 12.5 || toX == piece.PositionX + 12.5) 
            && toY == piece.PositionY + direction)
        {
            var target = FindPiece(toX, toY);
            if (target != null && target.Color != piece.Color)
            {
                return true;
            }              
        }

        return false;
    }

    private FigureViewModel? FindPiece(double toX, double toY)
        => _board.Figures.Find(f => f.PositionX == toX && f.PositionY == toY);
}
