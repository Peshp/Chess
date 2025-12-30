namespace Chess.Services.Services;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Chess.Data;
using Chess.Services.Services.Contracts;
using Chess.Web.ViewModels.Chess;
using Chess.Web.ViewModels.User;

using Microsoft.EntityFrameworkCore;

public class UserService : IUserService
{
    private readonly ChessDbContext context;

    public UserService(ChessDbContext context)
    {
        this.context = context;
    }

    public async Task<IEnumerable<UserBoardsViewModel>> GetHistory(string userId)
    {
        var boards = await this.context
            .UserBoards
            .Where(ub => ub.UserId == userId)
            .Select(b => new UserBoardsViewModel
            {
                BoardId = b.Id,
                UserId = b.UserId,
                Image = b.Image,
                Date = DateTime.Now.ToShortDateString(),
                MoveHistory = b.Squares.Select(m => new SquareViewModel
                {
                    FigureId = m.FigureId,
                    PositionX = m.PositionX,
                    PositionY = m.PositionY,
                    Coordinate = m.Coordinates,
                    FigureImage = m.FigureImage
                })
                .ToArray(),
                Figures = b.Boards.Select(f => new FigureViewModel
                {
                    Id = f.Id,
                    Name = f.Type.ToString(),
                    Color = f.Color.ToString(),
                    Image = f.Image,
                    PositionX = f.PositionX,
                    PositionY = f.PositionY,
                })
                .ToArray(),
            })
            .AsNoTracking()
            .ToArrayAsync();

        return boards;
    }

    public async Task<UserBoardsViewModel> GetBoardDetails(int boardId, string userId)
    {
        var board = await this.context.Boards.ToArrayAsync();
        var figures = await this.context.Figures.ToArrayAsync();

        IEnumerable<SquareViewModel> currentMoves = await this.context.BoardSquares
            .Where(sb => sb.BoardId == boardId)
            .Select(sb => new SquareViewModel
            {
                FigureId = sb.FigureId,
                PositionX = sb.PositionX,
                PositionY = sb.PositionY,
                Coordinate = sb.Coordinates,
                FigureImage = sb.FigureImage
            })
        .ToListAsync();

        UserBoardsViewModel viewModel = new UserBoardsViewModel
        {
            BoardId = boardId,
            Image = board[0].Image,
            UserId = userId,
            MoveHistory = (IList<SquareViewModel>)currentMoves,
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

    public async Task<UserBoardsViewModel> Next(UserBoardsViewModel board, int figureId, double toX, double toY)
    {
        var currentPiece = board.Figures.First(f => f.Id == figureId);

        var target = board.Figures.FirstOrDefault(f => f.PositionX == toX && f.PositionY == toY);

        if (target != null)
        {
            board.Figures.Remove(target);
        }

        currentPiece.PositionX = toX;
        currentPiece.PositionY = toY;

        return board;
    }
}
