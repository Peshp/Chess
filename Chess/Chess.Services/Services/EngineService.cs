namespace Chess.Services.Services;

using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Threading.Tasks;

using Chess.Services.Services.Contracts;
using Chess.Services.Validations.Engine;
using Chess.Web.ViewModels.Chess;

public class EngineService : IEngineService
{
    private readonly IEnumerable<IMoveValidator> validators;

    public EngineService(  
        IEnumerable<IMoveValidator> validators)
    {
        this.validators = validators;
    }

    public async Task<bool> TryMove(BoardViewModel board, int pieceId, double toX, double toY)
    {
        var piece = board.Figures.FirstOrDefault(f => f.Id == pieceId);
        if (piece.Color != board.CurrentTurn) return false;
        var validator = validators.FirstOrDefault(v => v.GetType().Name == piece.Name);
        King kingValidator = (King)validators.FirstOrDefault(v => v.GetType().Name == "King");

        if (!await validator.IsValidMoveAsync(piece, toX, toY, board)) return false;
        if (await kingValidator.IsSelfCheckAfterMove(board, piece, toX, toY)) return false;

        if(piece.Name == "King")
        {
            kingValidator.PerformCastleMove(board, piece, toX, toY);
            return true;
        }

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
        var kingValidator = (King)validators.FirstOrDefault(v => v.GetType().Name == "King");

        if (!await kingValidator.IsCheck(board, currentColor))
            return false;

        var myPieces = board.Figures.Where(f => f.Color == currentColor).ToList();

        foreach (var piece in myPieces)
        {
            var validator = validators.FirstOrDefault(v => v.GetType().Name == piece.Name);
            if (validator == null) continue;

            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    double targetX = x * 12.5;
                    double targetY = y * 12.5;

                    if (Math.Abs(piece.PositionX - targetX) < 0.1 && Math.Abs(piece.PositionY - targetY) < 0.1)
                        continue;

                    if (await validator.IsValidMoveAsync(piece, targetX, targetY, board))
                    {
                        if (!await kingValidator.IsSelfCheckAfterMove(board, piece, targetX, targetY))
                        {
                            return false;
                        }
                    }
                }
            }
        }

        return true;
    }

    public async Task<bool> IsInCheck(BoardViewModel board, string color)
    {
        King kingValidator = (King)validators.FirstOrDefault(v => v.GetType().Name == "King");

        return await kingValidator.IsCheck(board, color);
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
