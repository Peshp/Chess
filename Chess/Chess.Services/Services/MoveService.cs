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
    private readonly IEnumerable<IMoveValidator> validators;

    public MoveService(IEnumerable<IMoveValidator> validators)
    {
        this.validators = validators;
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
