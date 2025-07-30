namespace Chess.Contracts
{
    using Domain.Entities;

    public class ChessBoardViewModel
    {
        public IList<Figure> BoardState { get; set; } = new List<Figure>();

        public List<string> MoveHistory { get; set; } = new List<string>();

        public string CurrentTurn { get; set; }

        public string BoardImage { get; set; }

        public string PiecesPath { get; set; }

        public List<Figure> Figures { get; set; } = new List<Figure>();
    }
}
