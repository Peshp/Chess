#nullable disable

namespace Chess.Services.Services;

using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Threading.Tasks;

using Chess.Data;
using Chess.Services.Services.Contracts;
using Chess.Services.Validations.Engine;
using Chess.Web.ViewModels.Chess;

using Microsoft.EntityFrameworkCore;

public class EngineService : IEngineService
{
    private readonly ChessDbContext _context;
    private readonly IEnumerable<IMoveValidator> _validators;

    public EngineService(  
        IEnumerable<IMoveValidator> validators,
        ChessDbContext cpntext)
    {
        _context = cpntext;
        _validators = validators;
    }

    public async Task<bool> TryMove(BoardViewModel board, int pieceId, double toX, double toY)
    {
        var piece = board.Figures.FirstOrDefault(f => f.Id == pieceId);
        if (piece.Color != board.CurrentTurn) return false;
        var validator = _validators.FirstOrDefault(v => v.GetType().Name == piece.Name);

        if (!await validator.IsValidMoveAsync(piece, toX, toY, board)) return false;
        if (await this.IsSelfCheckAfterMove(board, piece, toX, toY)) return false;

        if (piece.Name == "King" &&
            validator is King kingValidator &&
            kingValidator.IsCastleAttempt(piece, toX, toY))
        {
            if (!await kingValidator.CanCastle(piece, board, toX, toY)) return false;
            if(await this.IsCheck(board, board.CurrentTurn)) return false;
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
        var validator = _validators.FirstOrDefault(v => v.GetType().Name == piece.Name);

        bool onEdge = false;
        if (piece.Name == "Pawn" && validator is Pawn pawnValidator)
        {
            onEdge = pawnValidator.OnEdge(piece);
        }

        return onEdge;
    }

    public async Task<bool> IsCheckmate(BoardViewModel board, string currentColor, string userId)
    {
        if (!await this.IsCheck(board, currentColor))
            return false;

        var myPieces = board.Figures.Where(f => f.Color == currentColor).ToList();

        foreach (var piece in myPieces)
        {
            var validator = _validators.FirstOrDefault(v => v.GetType().Name == piece.Name);
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
                        if (!await this.IsSelfCheckAfterMove(board, piece, targetX, targetY))
                        {
                            return false;
                        }
                    }
                }
            }
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

    public async Task<bool> IsCheck(BoardViewModel board, string color)
    {
        var king = board.Figures.FirstOrDefault(f => f.Name == "King" && f.Color == color);
        if (king == null) return false;

        var opponentColor = (color == "White") ? "Black" : "White";
        var opponentPieces = board.Figures.Where(f => f.Color == opponentColor);

        foreach (var piece in opponentPieces)
        {
            var validator = _validators.FirstOrDefault(v => v.GetType().Name == piece.Name);

            if (await validator.IsValidMoveAsync(piece, king.PositionX, king.PositionY, board))
                return true;
        }

        return false;
    }

    public async Task<bool> IsSelfCheckAfterMove(BoardViewModel board, FigureViewModel piece, double toX, double toY)
    {
        var originalX = piece.PositionX;
        var originalY = piece.PositionY;
        var captured = board.Figures.FirstOrDefault(f =>
            Math.Abs(f.PositionX - toX) < 0.1 && Math.Abs(f.PositionY - toY) < 0.1);

        if (captured != null) board.Figures.Remove(captured);
        piece.PositionX = toX;
        piece.PositionY = toY;

        bool leavesKingInCheck = await IsCheck(board, piece.Color);

        piece.PositionX = originalX;
        piece.PositionY = originalY;
        if (captured != null) board.Figures.Add(captured);

        return leavesKingInCheck;
    }
}
