namespace Chess.Web.ViewModels.Chess;

using System;
using System.Collections.Generic;

using global::Chess.Web.ViewModels.Contracts;

public class BoardViewModel : IBoardViewModel
{
    public int Id { get; set; }

    public string Image { get; set; }

    public string CurrentTurn { get; set; } = "White";

    public string UserId { get; set; }

    public string GameType { get; set; }

    public string Date { get; set; }

    public bool Success { get; set; }

    public bool IsCheck { get; set; }

    public bool IsGameOver { get; set; }

    public ClockViewModel WhiteClock { get; set; }

    public ClockViewModel BlackClock { get; set; }

    public ClockViewModel Clock { get; set; }

    public IList<SquareViewModel> MoveHistory { get; set; } = new List<SquareViewModel>();

    public IList<FigureViewModel> Figures { get; set; } = new List<FigureViewModel>();

    public IList<FigureViewModel> CapturedFigures { get; set; } = new List<FigureViewModel>();

    public object FiguresJson => Figures.Select(f => new
    {
        id = f.Id,
        x = f.PositionX,
        y = f.PositionY,
        name = f.Name,
        color = f.Color,
        image = f.Image,
        isMoved = f.IsMoved
    });

    public object CapturedJson => CapturedFigures.Select(f => new
    {
        color = f.Color,
        image = f.Image
    });

    public object HistoryJson => MoveHistory.Select(m => new
    {
        coordinate = m.Coordinate,
        figureImage = m.FigureImage
    });
}