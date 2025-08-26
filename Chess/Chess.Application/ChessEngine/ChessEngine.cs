namespace Chess.Application.ChessEngine;

using Application.ChessEngine.Validators;
using Domain.ViewModels.Web;

public class ChessEngine
{
    private BoardViewModel _board;
    private Dictionary<string, IMoveValidator> _moveValidators;

    public ChessEngine(BoardViewModel board)
    {
        _board = board;
        _moveValidators = new Dictionary<string, IMoveValidator>
        {
            { "Pawn", new PawnMoveValidator() },
            { "Bishop", new BishopMoveValidator() },
            { "Rook", new RookMoveValidator() },
            { "Queen", new QueenMoveValidator() },
            { "King", new KingMoveValidator() },
            { "Knight", new KnightMoveValidator() }
        };
    }

    public async Task<bool> TryMove(int pieceId, double toX, double toY)
    {
        var piece = _board.Figures.FirstOrDefault(f => f.Id == pieceId);
        if (piece == null) return false;
        if (piece.Color != _board.CurrentTurn) return false;
        if (!_moveValidators.TryGetValue(piece.Name, out var validator)) return false;

        if (piece.Name == "King" &&
            validator is KingMoveValidator kingValidator &&
            kingValidator.IsCastleAttempt(piece, toX, toY))
        {
            if (!kingValidator.CanCastle(piece, _board, toX, toY)) return false;
            if (!await IsCastleLegal(piece, toX, toY)) return false;
            PerformCastleMove(piece, toX, toY);
            _board.CurrentTurn = (_board.CurrentTurn == "White") ? "Black" : "White";
            return true;
        }

        if (!await IsValidMove(piece, toX, toY)) return false;
        if (await IsSelfCheckAfterMove(piece, toX, toY)) return false;

        var target = FindPiece(toX, toY);
        if (target != null && target.Color != piece.Color)
        {
            _board.CapturedFigures.Add(target);
            _board.Figures.Remove(target);
        }

        piece.PositionX = toX;
        piece.PositionY = toY;
        piece.IsMoved = true;

        _board.CurrentTurn = (_board.CurrentTurn == "White") ? "Black" : "White";
        return true;
    }

    private async Task<bool> IsSelfCheckAfterMove(FigureViewModel piece, double toX, double toY)
    {
        var originalX = piece.PositionX;
        var originalY = piece.PositionY;
        var captured = FindPiece(toX, toY);

        if (captured != null) _board.Figures.Remove(captured);
        piece.PositionX = toX;
        piece.PositionY = toY;

        bool leavesKingInCheck = await IsCheck(piece.Color);

        piece.PositionX = originalX;
        piece.PositionY = originalY;
        if (captured != null) _board.Figures.Add(captured);

        return leavesKingInCheck;
    }

    public async Task<bool> IsCheck(string color)
    {
        var king = _board.Figures.FirstOrDefault(f => f.Name == "King" && f.Color == color);
        if (king == null) return false;

        var opponentColor = (color == "White") ? "Black" : "White";
        var opponentPieces = _board.Figures.Where(f => f.Color == opponentColor);

        foreach (var piece in opponentPieces)
        {
            if (await IsValidMove(piece, king.PositionX, king.PositionY))
                return true;
        }
        return false;
    }

    private FigureViewModel? FindPiece(double x, double y)
        => _board.Figures.FirstOrDefault(f =>
            Math.Abs(f.PositionX - x) < 0.1 && Math.Abs(f.PositionY - y) < 0.1);

    private async Task<bool> IsValidMove(FigureViewModel piece, double toX, double toY)
    {
        if (_moveValidators.TryGetValue(piece.Name, out var validator))
            return validator.IsValidMove(piece, toX, toY, _board);
        return false;
    }

    private async Task<bool> IsCastleLegal(FigureViewModel king, double toX, double toY)
    {
        double direction = toX > king.PositionX ? 1 : -1;
        double step = 12.5 * direction;

        for (int i = 0; i <= 2; i++)
        {
            double x = king.PositionX + step * i;
            var originalX = king.PositionX;
            king.PositionX = x;
            bool inCheck = await IsCheck(king.Color);
            king.PositionX = originalX;
            if (inCheck) return false;
        }
        return true;
    }

    private void PerformCastleMove(FigureViewModel king, double toX, double toY)
    {
        double direction = toX > king.PositionX ? 1 : -1;
        double rookX = direction == 1 ? 87.5 : 0;
        double rookY = king.PositionY;
        var rook = _board.Figures.FirstOrDefault(f =>
            f.PositionX == rookX && f.PositionY == rookY && f.Color == king.Color && f.Name == "Rook");
        double toSquare = direction == 1 ? -12.5 : 12.5;

        king.PositionX = toX;
        king.PositionY = toY;
        king.IsMoved = true;
        if (rook != null)
        {
            rook.PositionX = toX + toSquare;
            rook.IsMoved = true;
        }
    }
}
