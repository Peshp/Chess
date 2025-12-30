namespace Chess.Services.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Chess.Data;
using Chess.Data.Models;
using Chess.Services.Services.Contracts;
using Chess.Web.ViewModels.Chess;

using Microsoft.EntityFrameworkCore;

public class GameService : IGameService
{
    private readonly ChessDbContext context;

    public GameService(ChessDbContext context)
    {
        this.context = context;
    }

    public async Task<BoardViewModel> GetBoard(ClockViewModel model, string userId)
    {
        var board = await this.context.Boards.ToArrayAsync();
        var figures = await this.context.Figures.ToArrayAsync();

        BoardViewModel viewModel = new BoardViewModel
        {
            Image = board[0].Image,
            UserId = userId,
            Clock = this.SetClock(model),
            WhiteClock = SetClock(model),
            BlackClock = SetClock(model),
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

    private ClockViewModel SetClock(ClockViewModel model)
    {
        ClockViewModel clock = new ClockViewModel();
        ClockViewModel whiteClock = new ClockViewModel();
        ClockViewModel blackClock = new ClockViewModel();

        clock.Minutes = model.Minutes;
        clock.Increment = model.Increment;
        whiteClock.Minutes = model.Minutes;
        blackClock.Minutes = model.Minutes;
        whiteClock.Increment = model.Increment;
        blackClock.Increment = model.Increment;

        return clock;
    }

    public async Task AddtoMoveHistory(BoardViewModel board, int pieceId, double toX, double toY)
    {
        FigureViewModel? currentPiece = board.Figures
            .FirstOrDefault(f => f.Id == pieceId);

        var model = await this.context.Squares
                .Where(s => s.PositionX == toX && s.PositionY == toY)
                .Select(s => new SquareViewModel
                {
                    FigureId = pieceId,
                    PositionX = s.PositionX,
                    PositionY = s.PositionY,
                    Coordinate = s.Coordinate,
                    FigureImage = currentPiece.Image,
                })
                .FirstOrDefaultAsync();

        board.MoveHistory.Add(model);
    }

    public async Task SaveBoard(BoardViewModel model, string userId)
    {
        var board = new UserBoard
        {
            Image = model.Image,
            UserId = userId,
        };

        var newFigures = model.Figures?
            .Select(f => new BoardFigures
            {
                Image = f.Image,
                PositionX = f.PositionX,
                PositionY = f.PositionY,
                BoardId = board.Id,
                Board = board,
            })
            .ToArray();

        var newSquares = model.MoveHistory?
            .Select(m => new BoardSquares
            {
                Coordinates = m.Coordinate,
                PositionX = m.PositionX,
                PositionY = m.PositionY,
                FigureId = m.FigureId,
                FigureImage = m.FigureImage,
                BoardId = board.Id,
                Board = board,
            })
            .ToArray();

        board.Squares = newSquares;
        board.Boards = newFigures;

        this.context.UserBoards.Add(board);
        this.context.SaveChanges();
    }
}
