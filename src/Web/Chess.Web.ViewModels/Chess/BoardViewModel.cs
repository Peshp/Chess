namespace Chess.Web.ViewModels.Chess
{
    using System;
    using System.Collections.Generic;

    public class BoardViewModel
    {

        public ICollection<FigureViewModel> Figures { get; set; }

        public ICollection<FigureViewModel> CapturedFigures { get; set; }

        public ICollection<SquareViewModel> MoveHistory { get; set; }

        public string BoardImage { get; set; }

        public string CurrentTurn { get; set; } = "White";

        public bool IsCheck { get; set; }
    }
}
