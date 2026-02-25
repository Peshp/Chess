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
    private readonly ICheckService checkService;
    private readonly IEnumerable<IMoveValidator> validators;

    public EngineService( 
        ICheckService checkService, 
        IEnumerable<IMoveValidator> validators)
    {
        this.checkService = checkService;
        this.validators = validators;
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
            if (!await kingValidator.CanCastle(piece, board, toX, toY)) return false;
            if (!await kingValidator.CanCastle(piece, board, toX, toY)) return false;
            kingValidator.PerformCastleMove(board, piece, toX, toY);
            board.CurrentTurn = (board.CurrentTurn == "White") ? "Black" : "White";
            return true;
        }

        if (!await validator.IsValidMoveAsync(piece, toX, toY, board)) return false;
        if (await checkService.IsSelfCheckAfterMove(board, piece, toX, toY)) return false;

        var target = await this.FindPiece(board, toX, toY);
        if (target != null && target.Color != piece.Color)
        {
            await this.CapturePiece(board, target);
        }
        else if (piece.Name == "Pawn" && target == null && Math.Abs(toX - piece.PositionX) > 0.1)
        {
            var enPassantTarget = board.Figures.FirstOrDefault(f => 
                Math.Abs(f.PositionX - toX) < 0.1 && 
                Math.Abs(f.PositionY - piece.PositionY) < 0.1 &&
                f.Color != piece.Color &&
                f.Name == "Pawn");
            
            if (enPassantTarget != null)
            {
                await this.CapturePiece(board, enPassantTarget);
            }
        }

        piece.PositionX = toX;
        piece.PositionY = toY;
        piece.IsMoved = true;

        board.CurrentTurn = (board.CurrentTurn == "White") ? "Black" : "White";
        return true;
    }

    public async Task<bool> PawnOnEdge(BoardViewModel board, int pieceId)
    {
        var piece = board.Figures.FirstOrDefault(f => f.Id == pieceId);
        var validator = validators.FirstOrDefault(v => v.GetType().Name == piece.Name);

        bool onEdge = false;
        if (piece.Name == "Pawn" && validator is Pawn pawnValidator)
        {
            onEdge = pawnValidator.OnEdge(piece);
        }

        return onEdge;
    }

    public async Task<bool> IsCheckmate(BoardViewModel board, string currentColor)
    {
        if (!await checkService.IsCheck(board, currentColor))
            return false;

        var legalMoves = new List<(FigureViewModel piece, double toX, double toY)>();

        foreach (var piece in board.Figures.Where(f => f.Color == currentColor))
        {
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    double toX = x * 12.5;
                    double toY = y * 12.5;
                    if (Math.Abs(piece.PositionX - toX) > 0.1 || Math.Abs(piece.PositionY - toY) > 0.1)
                    {
                        var validator = validators.FirstOrDefault(v => v.GetType().Name == piece.Name);
                        if (validator != null && await validator.IsValidMoveAsync(piece, toX, toY, board))
                        {
                            legalMoves.Add((piece, toX, toY));
                        }
                    }
                }
            }
        }

        foreach (var move in legalMoves)
        {
            if (!await checkService.IsSelfCheckAfterMove(board, move.piece, move.toX, move.toY))
                return false;
        }
        return true;
    }

    public async Task<FigureViewModel> FindPiece(BoardViewModel board, double x, double y)
        => board.Figures.FirstOrDefault(f =>
            Math.Abs(f.PositionX - x) < 0.1 && Math.Abs(f.PositionY - y) < 0.1);

    public async Task CapturePiece(BoardViewModel board, FigureViewModel target)
    {
        board.CapturedFigures.Add(target);
        board.Figures.Remove(target);
    }
}
