namespace Chess.Web.ViewModels.Chess
{
    using System;
    using System.Collections.Generic;

    public class BoardViewModel
    {
        public List<FigureViewModel> Figures { get; set; } = new();

        public List<FigureViewModel> CapturedFigures { get; set; } = new();

        public List<SquareViewModel> MoveHistory { get; set; } = new();

        public string BoardImage { get; set; }

        public string CurrentTurn { get; set; } = "White";

        public bool IsCheck { get; set; }
    }
}
