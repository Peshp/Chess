namespace Chess.Services.Helpers
{
    using Chess.Web.ViewModels.Chess;

    public static class ParseUciMove
    {
        public static (string? PieceId, double ToX, double ToY) FromUci(string uci, BoardViewModel board)
        {
            double fromX = (uci[0] - 'a') * 12.5;
            double fromY = (8 - (int)char.GetNumericValue(uci[1])) * 12.5;

            double targetX = (uci[2] - 'a') * 12.5;
            double targetY = (8 - (int)char.GetNumericValue(uci[3])) * 12.5;

            var piece = board.Figures.FirstOrDefault(f =>
                Math.Abs(f.PositionX - fromX) < 0.1 &&
                Math.Abs(f.PositionY - fromY) < 0.1);

            return (piece?.Id.ToString(), targetX, targetY);
        }
    }
}
