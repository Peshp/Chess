namespace Chess.Services.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Chess.Services.Services.Contracts;
using Chess.Services.Validations.Engine;
using Chess.Web.ViewModels.Chess;

public class MoveService : IMoveService
{
    private readonly IEnumerable<IMoveValidator> _validators;

    public MoveService(IEnumerable<IMoveValidator> validators) => _validators = validators;

    public async Task<bool> IsValidMove(BoardViewModel board, FigureViewModel piece, double toX, double toY)
    {
        var validator = _validators.FirstOrDefault(v => v.GetType().Name == piece.Name);

        return validator?.IsValidMove(piece, toX, toY, board) ?? false;
    }

    public FigureViewModel? FindPiece(BoardViewModel board, double x, double y)
        => board.Figures.FirstOrDefault(f =>
            Math.Abs(f.PositionX - x) < 0.1 && Math.Abs(f.PositionY - y) < 0.1);

    public void CapturePiece(BoardViewModel board, FigureViewModel target)
    {
        board.CapturedFigures.Add(target);
        board.Figures.Remove(target);
    }
}
