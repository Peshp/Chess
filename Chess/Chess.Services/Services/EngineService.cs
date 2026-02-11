namespace Chess.Services.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Chess.Services.Services.Contracts;
using Chess.Services.Validations.Engine;
using Chess.Web.ViewModels.Chess;

public class EngineService : IEngineService
{
    private readonly IMoveService moveService;
    private readonly ICheckService checkService;
    private readonly ICastleService castleService;
    private readonly IEnumerable<IMoveValidator> validators;

    public EngineService( 
        ICheckService checkService, 
        ICastleService castleService,
        IMoveService moveService,
        IEnumerable<IMoveValidator> validators)
    {
        this.checkService = checkService;
        this.castleService = castleService;
        this.validators = validators;
        this.moveService = moveService;
    }

    public async Task<bool> TryMove(BoardViewModel board, int pieceId, double toX, double toY)
    {
        var piece = board.Figures.FirstOrDefault(f => f.Id == pieceId);
        if (piece.Color != board.CurrentTurn) return false;
        var validator = validators.FirstOrDefault(v => v.GetType().Name == piece.Name);

        if (piece.Name == "King" &&
            validator is King kingValidator &&
            kingValidator.IsCastleAttempt(piece, toX, toY))
        {
            if (!kingValidator.CanCastle(piece, board, toX, toY)) return false;
            if (!await castleService.IsCastleLegal(board, piece, toX, toY, checkService)) return false;
            castleService.PerformCastleMove(board, piece, toX, toY);
            board.CurrentTurn = (board.CurrentTurn == "White") ? "Black" : "White";
            return true;
        }

        if (!validator.IsValidMove(piece, toX, toY, board)) return false;
        if (await checkService.IsSelfCheckAfterMove(board, piece, toX, toY)) return false;

        var target = moveService.FindPiece(board, toX, toY);
        if (target != null && target.Color != piece.Color)
        {
            moveService.CapturePiece(board, target);
        }

        piece.PositionX = toX;
        piece.PositionY = toY;
        piece.IsMoved = true;

        board.CurrentTurn = (board.CurrentTurn == "White") ? "Black" : "White";
        return true;
    }

    public async Task<bool> IsCheckmate(BoardViewModel board, string currentColor)
    {
        if (!await checkService.IsCheck(board, currentColor))
            return false;

        var legalMoves =
            board.Figures
            .Where(f => f.Color == currentColor)
            .SelectMany(piece =>
                Enumerable.Range(0, 8)
                    .SelectMany(x => Enumerable.Range(0, 8)
                        .Select(y => new { piece, toX = x * 12.5, toY = y * 12.5 })
                    )
            )
            .Where(m => (Math.Abs(m.piece.PositionX - m.toX) > 0.1 || Math.Abs(m.piece.PositionY - m.toY) > 0.1) &&
                        validators.FirstOrDefault(v => v.GetType().Name == m.piece.Name)
                        .IsValidMove(m.piece, m.toX, m.toY, board))
            .ToList();

        foreach (var move in legalMoves)
        {
            if (!await checkService.IsSelfCheckAfterMove(board, move.piece, move.toX, move.toY))
                return false;
        }
        return true;
    }
}
