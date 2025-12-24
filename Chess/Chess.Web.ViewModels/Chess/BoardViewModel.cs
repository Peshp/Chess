namespace Chess.Web.ViewModels.Chess;

using System;
using System.Collections.Generic;

using global::Chess.Web.ViewModels.Contracts;

public class BoardViewModel : IBoardViewModel
{
    public int Id { get; set; }

    public string Image { get; set; }

    public string CurrentTurn { get; set; } = "White";

    public bool IsCheck { get; set; }

    public string UserId { get; set; }

    public string Date { get; set; }

    public ClockViewModel WhiteClock { get; set; }

    public ClockViewModel BlackClock { get; set; }

    public ClockViewModel Clock { get; set; }

    public IList<SquareViewModel> MoveHistory { get; set; } = new List<SquareViewModel>();

    public IList<FigureViewModel> Figures { get; set; } = new List<FigureViewModel>();

    public IList<FigureViewModel> CapturedFigures { get; set; } = new List<FigureViewModel>();
}