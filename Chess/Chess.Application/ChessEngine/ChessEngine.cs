namespace Chess.Application.ChessEngine;

using Application.ChessEngine.Validators;
using Domain.ViewModels.Web;

public class ChessEngine
{
    private readonly BoardViewModel _board;
    private readonly Dictionary<string, IMoveValidator> _moveValidators;

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

        // Turn logic: Only allow move if it's that color's turn!
        if (piece.Color != _board.CurrentTurn)
            return false;

        if (await IsValidMove(piece, toX, toY))
        {
            var target = FindPiece(toX, toY);
            if (target != null && target.Color != piece.Color)
            {
                _board.CapturedFigures.Add(target);
                _board.Figures.Remove(target);
            }

            piece.PositionX = toX;
            piece.PositionY = toY;

            // Switch turn after successful move
            _board.CurrentTurn = (_board.CurrentTurn == "White") ? "Black" : "White";

            return true;
        }
        return false;
    }

    private async Task<bool> IsValidMove(FigureViewModel piece, double toX, double toY)
    {
        if (_moveValidators.TryGetValue(piece.Name, out var validator))
            return validator.IsValidMove(piece, toX, toY, _board);

        return false;
    }

    private FigureViewModel? FindPiece(double toX, double toY)
        => _board.Figures.FirstOrDefault(f => f.PositionX == toX && f.PositionY == toY);
}
