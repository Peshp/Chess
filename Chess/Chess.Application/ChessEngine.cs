namespace Chess.Application
{
    using infrastructure.Entities;

    public class ChessEngine
    {
        private List<Figure> _figures;

        public ChessEngine(List<Figure> figures)
        {
            _figures = figures;
        }

        public bool TryMove(int pieceId, int toX, int toY)
        {
            var piece = _figures.FirstOrDefault(f => f.Id == pieceId);
            if (piece == null) return false;

            if (IsValidMove(piece, toX, toY))
            {
                piece.PositionX = toX;
                piece.PositionY = toY;
                return true;
            }

            return false;
        }

        private bool IsValidMove(Figure piece, int toX, int toY)
        {
            // TODO: Add real chess rules here
            return true;
        }

        public List<Figure> GetUpdatedFigures() => _figures;
    }
}
