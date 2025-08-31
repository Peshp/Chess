namespace Chess.Services.Services
{
    using System.Threading.Tasks;

    using Chess.Data;

    public class GameService
    {
        private readonly ChessDbContext context;

        public GameService(ChessDbContext context)
        {
            this.context = context;
        }

        public async Task<BoardViewModel> GetBoard()
        {
            var board = await _context.Boards.Where(b => b.Id == 1).ToArrayAsync();
            var figures = await _context.Figures.ToArrayAsync();

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
                }).ToList()
            };

            return viewModel;
        }

        public async Task<bool> TryMove(BoardViewModel board, int pieceId, double toX, double toY)
        {
            ChessEngine engine = new ChessEngine(board);

            return await engine.TryMove(pieceId, toX, toY);
        }

        public async Task<bool> IsCheck(BoardViewModel board, string color)
        {
            var engine = new ChessEngine(board);
            return await engine.IsCheck(color);
        }
    }
}
