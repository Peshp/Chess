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

            if (target != null && target.Color != piece.Color) return true;
            if (target == null && await IsEnPassantMove(piece, toX, toY, board)) return true;
        }

        return false;
    }

    public bool OnEdge(FigureViewModel piece)
    {
        double lastSquareY = piece.Color == "White" ? 0 : 87.5;
        return Math.Abs(piece.PositionY - lastSquareY) < 0.1;
    }

    private async Task<bool> IsEnPassantMove(FigureViewModel piece, double toX, double toY, BoardViewModel board)
    {
        if (board.MoveHistory == null || board.MoveHistory.Count < 2)
            return false;

        var lastMove = board.MoveHistory[^1];
        var lastMoveFigure = board.Figures.FirstOrDefault(f => f.Id == lastMove.FigureId);

        if (lastMoveFigure == null || 
            lastMoveFigure.Name != "Pawn" || 
            lastMoveFigure.Color == piece.Color)
            return false;

        if (Math.Abs(lastMoveFigure.PositionY - piece.PositionY) > 0.1)
            return false;

        if (Math.Abs(lastMoveFigure.PositionX - piece.PositionX) - 12.5 > 0.1)
            return false;

        double direction = piece.Color == "White" ? -12.5 : 12.5;
        bool isCorrectTarget = Math.Abs(toX - lastMoveFigure.PositionX) < 0.1 && 
                               Math.Abs(toY - (lastMoveFigure.PositionY + direction)) < 0.1;

        return isCorrectTarget;
    }

    private bool IsEmptySquare(double x, double y, BoardViewModel board) =>
        !board.Figures.Any(f => Math.Abs(f.PositionX - x) < 0.1 && Math.Abs(f.PositionY - y) < 0.1);
}
