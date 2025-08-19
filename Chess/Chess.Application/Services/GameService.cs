namespace Chess.Application.Services;

using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Application;
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
}

