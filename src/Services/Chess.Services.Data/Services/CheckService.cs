namespace Chess.Services.Data.Services
{
    using Chess.Services.Data.Services.Contracts;
    using Chess.Web.ViewModels.Chess;
    using System.Linq;
    using System.Threading.Tasks;

    public class CheckService : ICheckService
    {
        public async Task<bool> IsCheck(BoardViewModel board, string color)
        {
            var king = board.Figures.FirstOrDefault(f => f.Name == "King" && f.Color == color);
            if (king == null)
            {
                return false;
            }

            string opponentColor = (color == "White") ? "Black" : "White";
            var opponentPieces = board.Figures.Where(f => f.Color == opponentColor);

            foreach (var piece in opponentPieces)
            {
                if (await IsValidMove(piece, king.PositionX, king.PositionY))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
