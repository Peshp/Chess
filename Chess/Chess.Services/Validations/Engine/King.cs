namespace Chess.Services.Validations.Engine;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chess.Services.Services.Contracts;
using Chess.Web.ViewModels.Chess;

/// <summary>
/// Validator for King piece movements including castling and check detection
/// </summary>
public class King : IMoveValidator
{
    #region Castling Logic

    public bool IsCastleAttempt(FigureViewModel king, double toX, double toY)
    {
        // Castling moves king exactly 2 squares horizontally (25 = 2 * 12.5)
        return Math.Abs(king.PositionY - toY) < 0.1 && Math.Abs(king.PositionX - toX) > 24.9;
    }

    public async Task<bool> CanCastle(FigureViewModel king, BoardViewModel board, double toX, double toY)
    {
        // Rule 1: King must not have moved
        if (king.IsMoved) 
            return false;

        // Rule 2: King must not be in check currently
        if (await IsCheck(board, king.Color))
            return false;

        var castleDirection = GetCastleDirection(king.PositionX, toX);
        var rook = FindCastlingRook(board, king, castleDirection);

        // Rule 3: Rook must exist and not have moved
        if (rook == null || rook.IsMoved)
            return false;

        // Rule 4: Path between king and rook must be clear
        if (!IsCastlePathClear(board, king, rook, castleDirection))
            return false;

        // Rule 5: King cannot pass through or land on attacked squares
        if (await IsKingPathUnderAttack(board, king, toX, castleDirection))
            return false;

        return true;
    }

    public void PerformCastleMove(BoardViewModel board, FigureViewModel king, double toX, double toY)
    {
        var direction = GetCastleDirection(king.PositionX, toX);
        var rook = FindCastlingRook(board, king, direction);

        if (rook == null)
            return;

        // Move king 2 squares
        king.PositionX = toX;
        king.IsMoved = true;

        // Move rook to opposite side of king
        double rookNewX = direction == CastleDirection.KingSide 
            ? toX - 12.5  // Rook goes to f-file (one square left of king)
            : toX + 12.5; // Rook goes to d-file (one square right of king)
        
        rook.PositionX = rookNewX;
        rook.IsMoved = true;
    }

    private CastleDirection GetCastleDirection(double fromX, double toX)
    {
        return toX > fromX ? CastleDirection.KingSide : CastleDirection.QueenSide;
    }

    private FigureViewModel FindCastlingRook(BoardViewModel board, FigureViewModel king, CastleDirection direction)
    {
        double rookX = direction == CastleDirection.KingSide ? 87.5 : 0;
        
        return board.Figures.FirstOrDefault(f =>
            Math.Abs(f.PositionX - rookX) < 0.1 && 
            Math.Abs(f.PositionY - king.PositionY) < 0.1 && 
            f.Color == king.Color && 
            f.Name == "Rook");
    }

    private bool IsCastlePathClear(BoardViewModel board, FigureViewModel king, FigureViewModel rook, CastleDirection direction)
    {
        double step = direction == CastleDirection.KingSide ? 12.5 : -12.5;
        double x = king.PositionX + step;
        double rookX = rook.PositionX;

        while (Math.Abs(x - rookX) > 0.1)
        {
            if (board.Figures.Any(f => 
                Math.Abs(f.PositionX - x) < 0.1 && 
                Math.Abs(f.PositionY - king.PositionY) < 0.1))
            {
                return false;
            }
            x += step;
        }

        return true;
    }

    private async Task<bool> IsKingPathUnderAttack(BoardViewModel board, FigureViewModel king, double toX, CastleDirection direction)
    {
        // Check all squares the king passes through (including destination)
        var squaresToCheck = GetCastlingSquares(king.PositionX, toX, king.PositionY, direction);

        foreach (var square in squaresToCheck)
        {
            if (await IsSquareUnderAttack(board, square.x, square.y, king.Color))
                return true;
        }

        return false;
    }

    private List<(double x, double y)> GetCastlingSquares(double fromX, double toX, double y, CastleDirection direction)
    {
        var squares = new List<(double x, double y)>();
        double step = direction == CastleDirection.KingSide ? 12.5 : -12.5;
        double x = fromX;

        // Include starting position, intermediate square, and destination
        while (Math.Abs(x - toX) > 0.1)
        {
            x += step;
            squares.Add((x, y));
        }

        return squares;
    }

    #endregion

    #region Move Validation

    public async Task<bool> IsValidMoveAsync(FigureViewModel piece, double toX, double toY, BoardViewModel board)
    {
        // Check for castling attempt first
        if (IsCastleAttempt(piece, toX, toY))
            return await CanCastle(piece, board, toX, toY);

        // Validate normal king movement (one square in any direction)
        if (!IsValidKingStep(piece.PositionX, piece.PositionY, toX, toY))
            return false;

        // Check if destination is occupied by own piece
        var target = board.Figures.FirstOrDefault(f => 
            Math.Abs(f.PositionX - toX) < 0.1 && 
            Math.Abs(f.PositionY - toY) < 0.1);
        
        if (target != null && target.Color == piece.Color)
            return false;

        // King cannot move into check
        return !await WouldBeInCheck(board, piece, toX, toY);
    }

    private bool IsValidKingStep(double fromX, double fromY, double toX, double toY)
    {
        double dx = Math.Abs(fromX - toX);
        double dy = Math.Abs(fromY - toY);
        
        // Must move exactly one square
        return dx <= 12.5 && dy <= 12.5 && (dx + dy) > 0;
    }

    #endregion

    #region Check Detection

    public async Task<bool> IsCheck(BoardViewModel board, string color)
    {
        var king = board.Figures.FirstOrDefault(f => f.Name == "King" && f.Color == color);
        if (king == null) 
            return false;

        return await IsSquareUnderAttack(board, king.PositionX, king.PositionY, color);
    }

    public async Task<bool> IsSquareUnderAttack(BoardViewModel board, double x, double y, string defendingColor)
    {
        var opponentColor = defendingColor == "White" ? "Black" : "White";
        var opponentPieces = board.Figures.Where(f => f.Color == opponentColor);

        foreach (var attacker in opponentPieces)
        {
            if (CanPieceAttackSquare(attacker, x, y, board))
                return true;
        }

        return false;
    }

    public async Task<bool> IsSelfCheckAfterMove(BoardViewModel board, FigureViewModel piece, double toX, double toY)
    {
        return await WouldBeInCheck(board, piece, toX, toY);
    }

    private async Task<bool> WouldBeInCheck(BoardViewModel board, FigureViewModel piece, double toX, double toY)
    {
        // Simulate the move
        var originalX = piece.PositionX;
        var originalY = piece.PositionY;
        var captured = board.Figures.FirstOrDefault(f =>
            Math.Abs(f.PositionX - toX) < 0.1 && 
            Math.Abs(f.PositionY - toY) < 0.1);

        if (captured != null) 
            board.Figures.Remove(captured);
        
        piece.PositionX = toX;
        piece.PositionY = toY;

        // Check if king would be in check
        bool wouldBeInCheck = await IsCheck(board, piece.Color);

        // Restore original state
        piece.PositionX = originalX;
        piece.PositionY = originalY;
        if (captured != null) 
            board.Figures.Add(captured);

        return wouldBeInCheck;
    }

    #endregion

    #region Attack Pattern Detection

    private bool CanPieceAttackSquare(FigureViewModel attacker, double targetX, double targetY, BoardViewModel board)
    {
        double dx = Math.Abs(attacker.PositionX - targetX);
        double dy = Math.Abs(attacker.PositionY - targetY);

        return attacker.Name switch
        {
            "Pawn" => CanPawnAttack(attacker, targetX, targetY),
            "Knight" => CanKnightAttack(dx, dy),
            "Bishop" => CanBishopAttack(attacker.PositionX, attacker.PositionY, targetX, targetY, dx, dy, board),
            "Rook" => CanRookAttack(attacker.PositionX, attacker.PositionY, targetX, targetY, dx, dy, board),
            "Queen" => CanQueenAttack(attacker.PositionX, attacker.PositionY, targetX, targetY, dx, dy, board),
            "King" => CanKingAttack(dx, dy),
            _ => false
        };
    }

    private bool CanPawnAttack(FigureViewModel pawn, double targetX, double targetY)
    {
        double direction = pawn.Color == "White" ? -12.5 : 12.5;
        double expectedY = pawn.PositionY + direction;
        
        return Math.Abs(targetX - pawn.PositionX) > 11 && 
               Math.Abs(targetX - pawn.PositionX) < 13 && 
               Math.Abs(targetY - expectedY) < 0.1;
    }

    private bool CanKnightAttack(double dx, double dy)
    {
        return (Math.Abs(dx - 12.5) < 0.1 && Math.Abs(dy - 25) < 0.1) || 
               (Math.Abs(dx - 25) < 0.1 && Math.Abs(dy - 12.5) < 0.1);
    }

    private bool CanBishopAttack(double fromX, double fromY, double toX, double toY, double dx, double dy, BoardViewModel board)
    {
        if (Math.Abs(dx - dy) > 0.1 || dx < 0.1) 
            return false;
        
        return !IsPathBlocked(fromX, fromY, toX, toY, board);
    }

    private bool CanRookAttack(double fromX, double fromY, double toX, double toY, double dx, double dy, BoardViewModel board)
    {
        bool isStraightLine = (dx < 0.1 && dy > 0.1) || (dy < 0.1 && dx > 0.1);
        if (!isStraightLine) 
            return false;
        
        return !IsPathBlocked(fromX, fromY, toX, toY, board);
    }

    private bool CanQueenAttack(double fromX, double fromY, double toX, double toY, double dx, double dy, BoardViewModel board)
    {
        // Queen combines rook and bishop movements
        bool isDiagonal = Math.Abs(dx - dy) < 0.1 && dx > 0.1;
        bool isStraightLine = (dx < 0.1 && dy > 0.1) || (dy < 0.1 && dx > 0.1);
        
        if (!isDiagonal && !isStraightLine)
            return false;
        
        return !IsPathBlocked(fromX, fromY, toX, toY, board);
    }

    private bool CanKingAttack(double dx, double dy)
    {
        return dx <= 12.5 && dy <= 12.5 && (dx + dy) > 0.1;
    }

    private bool IsPathBlocked(double fromX, double fromY, double toX, double toY, BoardViewModel board)
    {
        double dx = toX - fromX;
        double dy = toY - fromY;
        double steps = Math.Max(Math.Abs(dx), Math.Abs(dy)) / 12.5;

        if (steps <= 1) 
            return false;

        double stepX = dx == 0 ? 0 : (dx / Math.Abs(dx)) * 12.5;
        double stepY = dy == 0 ? 0 : (dy / Math.Abs(dy)) * 12.5;

        double x = fromX + stepX;
        double y = fromY + stepY;

        for (int i = 1; i < steps; i++)
        {
            if (board.Figures.Any(f => 
                Math.Abs(f.PositionX - x) < 0.1 && 
                Math.Abs(f.PositionY - y) < 0.1))
            {
                return true;
            }
            x += stepX;
            y += stepY;
        }

        return false;
    }

    #endregion

    #region Helper Types

    private enum CastleDirection
    {
        KingSide,   
        QueenSide   
    }

    #endregion
}
