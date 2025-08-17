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

        public async Task<bool> TryMove(int pieceId, double toX, double toY)
        {
            var piece = _figures.FirstOrDefault(f => f.Id == pieceId);

            if (IsValidMove(piece, toX, toY).Result)
            {
                piece.PositionX = toX;
                piece.PositionY = toY;

                return true;
            }

            return false;
        }

        private async Task<bool> IsValidMove(FigureViewModel piece, double toX, double toY)
        {
            // TODO: real chess rules
            bool valid = await IsEmpty(piece, toX, toY);

            return valid;
        }

        private async Task<bool> IsEmpty(FigureViewModel piece, double toX, double toY)
        {
            bool target = _figures.Find(f => f.PositionX == toX && f.PositionY == toY) == null;

            return target; 
        }
    }
}
