namespace Chess.Services.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Chess.Data;
    using Chess.Data.Models;
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
            var board = await this.context.Boards.ToArrayAsync();
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

        public async Task AddtoMoveHistory(BoardViewModel board, int pieceId, double toX, double toY)
        {
            FigureViewModel currentPiece = board.Figures
                .FirstOrDefault(f => f.Id == pieceId);

            var model = this.context.Squares
                    .Where(s => s.PositionX == toX && s.PositionY == toY)
                    .Select(s => new SquareViewModel
                    {
                        PositionX = s.PositionX,
                        PositionY = s.PositionY,
                        Coordinate = s.Coordinate,
                        FigureImage = currentPiece.Image,
                    })
                    .FirstOrDefault();

            board.MoveHistory.Add(model);
        }

        public async Task SaveBoard(BoardViewModel model)
        {
            Board board = new Board()
            {
                Id = model.Id,
                Image = model.BoardImage,
                Movehistory = model.MoveHistory.Select(m => new Square
                {
                    PositionX = m.PositionX,
                    PositionY = m.PositionY,
                    Coordinate = m.Coordinate,
                    BoardId = model.Id,
                })
                .ToList(),
            };

            await context.Boards.AddAsync(board);
            await context.SaveChangesAsync();
        }
    }
}
