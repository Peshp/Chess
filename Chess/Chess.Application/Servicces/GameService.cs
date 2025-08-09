namespace Chess.Application.Servicces
{
    using Microsoft.EntityFrameworkCore;
    using System.Threading.Tasks;

    using Application.interfaces;
    using Domain.ViewModels.Web;
    using Infrastructure.Data;

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

            var groupedFigures = figures
                .GroupBy(f => new { f.Color, f.Name })
                .SelectMany(g => g.Select((f, index) => new { Figure = f, Count = index + 1 }))
                .ToList();

            var piecePositions = new Dictionary<(string Color, string Name, int Count), (double X, double Y)>
            {
                // White pieces
                { ("White", "Rook", 1), (0, 7) },
                { ("White", "Knight", 1), (1, 7) },
                { ("White", "Bishop", 1), (2, 7) },
                { ("White", "Queen", 1), (3, 7) },
                { ("White", "King", 1), (4, 7) },
                { ("White", "Bishop", 2), (5, 7) },
                { ("White", "Knight", 2), (6, 7) },
                { ("White", "Rook", 2), (7, 7) },
                { ("White", "Pawn", 1), (0, 6) },
                { ("White", "Pawn", 2), (1, 6) },
                { ("White", "Pawn", 3), (2, 6) },
                { ("White", "Pawn", 4), (3, 6) },
                { ("White", "Pawn", 5), (4, 6) },
                { ("White", "Pawn", 6), (5, 6) },
                { ("White", "Pawn", 7), (6, 6) },
                { ("White", "Pawn", 8), (7, 6) },

                // Black pieces
                { ("Black", "Rook", 1), (0, 0) },
                { ("Black", "Knight", 1), (1, 0) },
                { ("Black", "Bishop", 1), (2, 0) },
                { ("Black", "Queen", 1), (3, 0) },
                { ("Black", "King", 1), (4, 0) },
                { ("Black", "Bishop", 2), (5, 0) },
                { ("Black", "Knight", 2), (6, 0) },
                { ("Black", "Rook", 2), (7, 0) },
                { ("Black", "Pawn", 1), (0, 1) },
                { ("Black", "Pawn", 2), (1, 1) },
                { ("Black", "Pawn", 3), (2, 1) },
                { ("Black", "Pawn", 4), (3, 1) },
                { ("Black", "Pawn", 5), (4, 1) },
                { ("Black", "Pawn", 6), (5, 1) },
                { ("Black", "Pawn", 7), (6, 1) },
                { ("Black", "Pawn", 8), (7, 1) },
            };

            BoardViewModel viewModel = new BoardViewModel
            {
                BoardImage = board[0].Image,
                Figures = groupedFigures.Select(entry =>
                {
                    var key = (entry.Figure.Color, entry.Figure.Name, entry.Count);
                    var pos = piecePositions.ContainsKey(key) ? piecePositions[key] : (entry.Count * 1.0, 0.0); // fallback to spread out

                    return new FigureViewModel
                    {
                        Id = entry.Figure.Id,
                        Name = entry.Figure.Name,
                        Color = entry.Figure.Color,
                        Image = entry.Figure.Image,
                        PositionX = pos.Item1 * 12.5,
                        PositionY = pos.Item2 * 12.5,
                    };
                }).ToList()
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

