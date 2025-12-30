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
        string userId = string.Empty;
        HttpContext.Session.Clear();

        if (User.Identity.IsAuthenticated)
        {
            userId = User.GetId();
        }

        UserBoardsViewModel board = HttpContext.Session.GetBoard<UserBoardsViewModel>();
        if (board == null)
        {
            board = await userService.GetBoardDetails(boardId, userId);
            HttpContext.Session.SetBoard(board);
        }

        return View(board);
    }

    [HttpPost]
    public async Task<IActionResult> Details(int figureId, double toX, double toY)
    {
        var board = HttpContext.Session.GetBoard<UserBoardsViewModel>();
        var boardUpdated = await userService.Next(board, figureId, toX, toY);

        HttpContext.Session.SetBoard(boardUpdated);

        return View(board);
    }
}
