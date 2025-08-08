namespace Chess.Application.Servicces
{
    using Application.interfaces;
    using Chess.infrastructure.Entities;
    using Domain.ViewModels.Web;
    using Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;
    using System.Threading.Tasks;

    public class GameService : IGameService
    {
        private readonly ApplicationDbContext _context;

        public GameService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<BoardViewModel> GetBoard()
        {
            var board = await _context.Boards.Where(b => b.Id == 1).ToArrayAsync();
            var figures = await _context.Figures.ToArrayAsync();

            BoardViewModel viewModel = new BoardViewModel
            {
                BoardImage = board[0].Image,
                Figures = figures.Select(figure => new FigureViewModel
                {
                    Id = figure.Id,
                    Name = figure.Name,
                    Color = figure.Color,
                    Image = figure.Image,
                    PositionX = figure.PositionX,
                    PositionY = figure.PositionY
                }).ToList(),
            };

            return viewModel;
        }

        public async Task<List<FigureViewModel>> GetFigures()
        {
            var figures = await _context.Figures.ToArrayAsync();

            var viewModels = figures.Select(figure => new FigureViewModel
            {
                Id = figure.Id,
                Name = figure.Name,
                Color = figure.Color,
                Image = figure.Image,
                PositionX = figure.PositionX,
                PositionY = figure.PositionY
            }).ToList();

            return viewModels;
        }
    }
}

