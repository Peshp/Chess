namespace Chess.Web.Controllers;

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

using Chess.Services.Services.Contracts;
using Chess.Web.Infrastructure.Extension;
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
}
