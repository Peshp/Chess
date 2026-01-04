namespace Chess.Web.Controllers;

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

using Chess.Data.Models;
using Chess.Services;
using Chess.Services.Services.Contracts;
using Chess.Web.Infrastructure.Extension;
using static Chess.Web.Infrastructure.Extension.CurrentSessionExtension;
using Chess.Web.ViewModels.Chess;
using Chess.Web.ViewModels.User;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

[Authorize]
public class UserController : BaseController
{
    private readonly IUserService userService;

    public UserController(IUserService userService)
    {
        this.userService = userService;
    }

    public async Task<IActionResult> Profile()
    {
        string? userId = User.GetId();

        IEnumerable<UserBoardsViewModel> boards = await userService.GetHistory(userId);

        return View(boards);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int boardId)
    {
        HttpContext.Session.Clear();
        string? userId = User.GetId();

        var board = await userService.GetBoardDetails(boardId, userId);

        board.Step = 0;

        HttpContext.Session.SetBoard(board);
        return View(board);
    }

    [HttpPost]
    public async Task<IActionResult> Details()
    {
        var board = HttpContext.Session.GetBoard<UserBoardsViewModel>();

        var nextMove = board.MoveHistory.ElementAtOrDefault(board.Step);

        if (nextMove != null)
        {
            board = await userService.Next(board, nextMove.FigureId, nextMove.PositionX, nextMove.PositionY);

            board.Step++;

            HttpContext.Session.SetBoard(board);
        }

        return View(board);
    }
}
