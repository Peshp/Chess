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

    public async Task<IEnumerable<UserBoardsViewModel>> GetHistory()
    {
        var boards = await this.context
            .UserBoards
            .Select(b => new UserBoardsViewModel
            {
                Id = b.Id,
                UserId = b.UserId,
                Image = b.Image,
                BoardId = b.Id,
                MoveHistory = b.Squares.Select(m => new SquareViewModel
                {
                    PositionX = m.PositionX,
                    PositionY = m.PositionY,
                    Coordinate = m.Coordinates,
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
}
