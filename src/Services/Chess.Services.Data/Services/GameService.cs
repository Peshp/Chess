namespace Chess.Services.Services
{
    using System.Collections.Generic;
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
            var board = await context.Boards
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == 1);

            var figures = await context.Figures
                .AsNoTracking()
                .Select(entry => new FigureViewModel
                {
                    Id = entry.Id,
                    Name = entry.Type.ToString(),
                    Color = entry.Color.ToString(),
                    IsMoved = false,
                    Image = entry.Image,
                    PositionX = entry.PositionX,
                    PositionY = entry.PositionY,
                })
                .ToListAsync();

            var viewModel = new BoardViewModel
            {
                BoardImage = board.Image,
                Figures = figures,
                CapturedFigures = new List<FigureViewModel>(),
                MoveHistory = new List<SquareViewModel>(),
            };

            return viewModel;
        }

        public async Task<bool> TryMove(BoardViewModel board, int pieceId, double toX, double toY)
        {
            EngineService engine = new EngineService(board);
            bool success = await engine.TryMove(pieceId, toX, toY);

            FigureViewModel currentPiece = board.Figures
                .FirstOrDefault(f => f.Id == pieceId);

            if (success)
            {
                var model = await this.context.Squares
                    .Where(s => s.PositionX == toX && s.PositionY == toY)
                    .Select(s => new SquareViewModel
                    {
                        PositionX = s.PositionX,
                        PositionY = s.PositionY,
                        Coordinate = s.Coordinate,
                        FigureImage = currentPiece.Image,
                    })
                    .FirstOrDefaultAsync();

                board.MoveHistory.Add(model);
            }

            return success;
        }

        public async Task<bool> IsCheck(BoardViewModel board, string color)
        {
            var engine = new EngineService(board);
            return await engine.IsCheck(color);
        }
    }
}
