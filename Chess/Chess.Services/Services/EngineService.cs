namespace Chess.Services.Services;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

using Chess.Services.Services.Contracts;
using Chess.Services.Validations.Engine;
using Chess.Web.ViewModels.Chess;

public class EngineService : IEngineService
{
    private readonly IMoveService _moveService;
    private readonly ICheckService _checkService;
    private readonly ICastleService _castleService;
    private readonly IEnumerable<IMoveValidator> _validators;

    public EngineService(
        IMoveService moveService,
        ICheckService checkService,
        ICastleService castleService,
        IEnumerable<IMoveValidator> validators)
    {
        _moveService = moveService;
        _checkService = checkService;
        _castleService = castleService;
        _validators = validators;
    }

    public async Task<bool> TryMove(BoardViewModel board, int pieceId, double toX, double toY)
    {
        var piece = board.Figures.FirstOrDefault(f => f.Id == pieceId);
        if (piece.Color != board.CurrentTurn) return false;
        var validator = _validators.Where(v => v.GetType().Name == piece.Name);

        if (piece.Name == "King" &&
            validator is King kingValidator &&
            kingValidator.IsCastleAttempt(piece, toX, toY))
        {
            if (!kingValidator.CanCastle(piece, board, toX, toY)) return false;
            if (!await _castleService.IsCastleLegal(board, piece, toX, toY, _checkService)) return false;
            _castleService.PerformCastleMove(board, piece, toX, toY);
            board.CurrentTurn = (board.CurrentTurn == "White") ? "Black" : "White";
            return true;
        }

        if (!await _moveService.IsValidMove(board, piece, toX, toY)) return false;
        if (await _checkService.IsSelfCheckAfterMove(board, piece, toX, toY, _moveService)) return false;

        var target = _moveService.FindPiece(board, toX, toY);
        if (target != null && target.Color != piece.Color)
        {
            _moveService.CapturePiece(board, target);
        }

        piece.PositionX = toX;
        piece.PositionY = toY;
        piece.IsMoved = true;

        board.CurrentTurn = (board.CurrentTurn == "White") ? "Black" : "White";
        return true;
    }

    public async Task<bool> IsCheckmate(BoardViewModel board, string currentColor)
    {
        if (!await _checkService.IsCheck(board, currentColor)) return false;

        var myPieces = board.Figures.Where(f => f.Color == currentColor).ToList();

        for (double x = 0; x <= 87.5; x += 12.5)
        {
            for (double y = 0; y <= 87.5; y += 12.5)
            {
                foreach (var piece in myPieces)
                {
                    if (await _moveService.IsValidMove(board, piece, x, y))
                    {
                        if (!await _checkService.IsSelfCheckAfterMove(board, piece, x, y, _moveService))
                            return false;
                    }
                }
            }
        }
        return true;
    }
}
