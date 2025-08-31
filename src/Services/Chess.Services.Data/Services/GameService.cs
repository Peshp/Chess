namespace Chess.Services.Services
{
    using System.Linq;
    using System.Threading.Tasks;

    using Chess.Data;
    using Chess.Services.Data.Services;
    using Chess.Services.Data.Services.Contracts;
    using Chess.Web.ViewModels.Chess;
    using Microsoft.EntityFrameworkCore;

    public class GameService : IGameService
    {
        private readonly ChessDbContext context;

        public GameService(ChessDbContext context)
        {
            this.context = context;
        }

        public async Task<BoardViewModel> GetBoard()
        {
            var board = await this.context.Boards.Where(b => b.Id == 1).ToArrayAsync();
            var figures = await this.context.Figures.ToArrayAsync();

            BoardViewModel viewModel = new BoardViewModel
            {
                BoardImage = board[0].Image,
                Figures = figures.Select(entry =>
                {
                    return new FigureViewModel
                    {
                        Id = entry.Id,
                        Name = entry.Type.ToString(),
                        Color = entry.Color.ToString(),
                        IsMoved = false,
                        Image = entry.Image,
                        PositionX = entry.PositionX,
                        PositionY = entry.PositionY,
                    };
                })
                .ToList(),
            };

            return viewModel;
        }

        public async Task<bool> TryMove(BoardViewModel board, int pieceId, double toX, double toY)
        {
            EngineService engine = new EngineService(board);

            return await engine.TryMove(pieceId, toX, toY);
        }

        public async Task<bool> IsCheck(BoardViewModel board, string color)
        {
            var engine = new EngineService(board);
            return await engine.IsCheck(color);
        }
    }
}
