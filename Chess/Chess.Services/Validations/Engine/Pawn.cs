namespace Chess.Services.Validations.Engine;

using System.Linq;

using Chess.Web.ViewModels.Chess;

using Microsoft.IdentityModel.Tokens;

public class Pawn : IMoveValidator
{
    public async Task<bool> IsValidMoveAsync(FigureViewModel piece, double toX, double toY, BoardViewModel board)
    {
        double direction = piece.Color == "White" ? -12.5 : 12.5;
        double startRow = piece.Color == "White" ? 75 : 12.5;

        if (piece.PositionX == toX && piece.PositionY + direction == toY)
        {
            if (IsEmptySquare(toX, toY, board)) return true;
        }

        if (piece.PositionX == toX && piece.PositionY == startRow && piece.PositionY + direction * 2 == toY)
        {
            double intermediateY = piece.PositionY + direction;
            if (this.IsEmptySquare(toX, intermediateY, board) && IsEmptySquare(toX, toY, board)) return true;
        }

        if (Math.Abs(toX - piece.PositionX) == 12.5 && toY == piece.PositionY + direction)
        {
            var target = board.Figures.FirstOrDefault(f => f.PositionX == toX && f.PositionY == toY);

            // Standard Capture
            if (target != null && target.Color != piece.Color) return true;

            if (target == null && await IsEnPassantMove(piece, toX, toY, board)) return true;
        }

        return false;
    }

    private async Task<bool> IsEnPassantMove(FigureViewModel piece, double toX, double toY, BoardViewModel board)
    {
        // En passant is only valid if there's a move history
        if (board.MoveHistory == null || board.MoveHistory.Count < 2)
            return false;

        // Get the last move
        var lastMove = board.MoveHistory[^1];
        var lastMoveFigure = board.Figures.FirstOrDefault(f => f.Id == lastMove.FigureId);

        // En passant is only valid if the last move was a pawn of opposite color
        if (lastMoveFigure == null || 
            lastMoveFigure.Name != "Pawn" || 
            lastMoveFigure.Color == piece.Color)
            return false;


        

        // The enemy pawn must be on the same rank (Y position) as our pawn
        if (Math.Abs(lastMoveFigure.PositionY - piece.PositionY) > 0.1)
            return false;

        // The enemy pawn must be on an adjacent file (X position)
        if (Math.Abs(lastMoveFigure.PositionX - piece.PositionX) - 12.5 > 0.1)
            return false;

        // The target square must be behind the enemy pawn
        double direction = piece.Color == "White" ? -12.5 : 12.5;
        bool isCorrectTarget = Math.Abs(toX - lastMoveFigure.PositionX) < 0.1 && 
                               Math.Abs(toY - (lastMoveFigure.PositionY + direction)) < 0.1;

        return isCorrectTarget;
    }

    private bool IsEmptySquare(double x, double y, BoardViewModel board) =>
        !board.Figures.Any(f => Math.Abs(f.PositionX - x) < 0.1 && Math.Abs(f.PositionY - y) < 0.1);
}
