namespace Chess.Application
{
    using Domain.ViewModels.Web;
    //using infrastructure.Entities;

    public class ChessEngine
    {
        private List<FigureViewModel> _figures;

        public ChessEngine(List<FigureViewModel> figures)
        {
            _figures = figures;
        }

        public bool TryMove(int pieceId, double toX, double toY)
        {
            var piece = _figures.FirstOrDefault(f => f.Id == pieceId);

            if (IsValidMove(piece, toX, toY))
            {
                piece.PositionX = toX;
                piece.PositionY = toY;
                return true;
            }

            return false;
        }

        private bool IsValidMove(FigureViewModel piece, double toX, double toY)
        {
            // TODO: Add real chess rules her
            bool valid = true;
            if (piece.Name == "Pawn")
                valid = Pawn(piece, toX, toY);

            return valid;
        }

        public List<FigureViewModel> GetUpdatedFigures() => _figures;

        private bool Pawn(FigureViewModel pawn, double toX, double toY)
        {
            FigureViewModel isEmpty = _figures.FirstOrDefault(f =>
                f.PositionX == toX &&
                f.PositionY == toY);

            return true;
        }
    }
}
