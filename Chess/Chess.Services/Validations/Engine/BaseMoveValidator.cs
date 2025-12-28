namespace Chess.Services.Validations.Engine
{
    using Chess.Web.ViewModels.Chess;

    public abstract class BaseMoveValidator : IMoveValidator
    {
        public abstract bool IsValidMove(FigureViewModel piece, double toX, double toY, BoardViewModel board);

        protected bool IsPathClear(double fromX, double fromY, double toX, double toY, BoardViewModel board)
        {
            double dx = Math.Abs(toX - fromX) < 0.1 ? 0 : (toX > fromX ? 12.5 : -12.5);
            double dy = Math.Abs(toY - fromY) < 0.1 ? 0 : (toY > fromY ? 12.5 : -12.5);

            double x = fromX + dx;
            double y = fromY + dy;

            while (Math.Abs(x - toX) > 0.1 || Math.Abs(y - toY) > 0.1)
            {
                if (board.Figures.Any(f => Math.Abs(f.PositionX - x) < 0.1 && Math.Abs(f.PositionY - y) < 0.1))
                    return false;
                x += dx;
                y += dy;
            }
            return true;
        }

        protected bool IsFriendlyFire(FigureViewModel piece, double toX, double toY, BoardViewModel board)
        {
            var target = board.Figures.FirstOrDefault(f => Math.Abs(f.PositionX - toX) < 0.1 && Math.Abs(f.PositionY - toY) < 0.1);
            return target != null && target.Color == piece.Color;
        }
    }
}
