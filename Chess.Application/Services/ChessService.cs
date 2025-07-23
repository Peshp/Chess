namespace Chess.Application.Services
{
    using System.Threading.Tasks;

    using Web.ViesModels;
    using Infrastructure;
    using Interfaces;

    public class ChessService : IChessService
    {
        private readonly ChessDbContext dbContext;

        public async Task Game()
        {
            var viewModel = new ChessBoardViewModel
            {
                MoveHistory = new List<string>(),
                CurrentTurn = "White",
                BoardImage = dbContext.Boards.FirstOrDefault()?.Image,
                PiecesPath = "~/images/pieces/",
                Figures = dbContext.Figures.ToList()
            };
        }
    }
}
