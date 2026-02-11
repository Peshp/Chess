namespace Chess.Services.Services;

using Chess.Services.Services.Contracts;
using Chess.Services.Validations.Engine;
using Chess.Web.ViewModels.Chess;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class CheckService : ICheckService
{
    private readonly IEnumerable<IMoveValidator> validators;
    private readonly IMoveService moveService;

    public CheckService(IEnumerable<IMoveValidator> validators, IMoveService service)
    {
        this.validators = validators;
        moveService = service;
    }

    public async Task<bool> IsCheck(BoardViewModel board, string color)
    {
        var king = board.Figures.FirstOrDefault(f => f.Name == "King" && f.Color == color);
        if (king == null) return false;

        var opponentColor = (color == "White") ? "Black" : "White";
        var opponentPieces = board.Figures.Where(f => f.Color == opponentColor);

        foreach (var piece in opponentPieces)
        {
            var validator = validators.FirstOrDefault(v => v.GetType().Name == piece.Name);

            if (validator.IsValidMove(piece, king.PositionX, king.PositionY, board))
                return true;
        }

        return false;
    }

    public async Task<bool> IsSelfCheckAfterMove(BoardViewModel board, FigureViewModel piece, double toX, double toY)
    {
        var originalX = piece.PositionX;
        var originalY = piece.PositionY;
        var captured = moveService.FindPiece(board, toX, toY);

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
