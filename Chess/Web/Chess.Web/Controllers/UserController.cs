namespace Chess.Web.Controllers;

using System.Collections.Generic;
using System.Threading.Tasks;

using Chess.Services.Services.Contracts;
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
        IEnumerable<UserBoardsViewModel> boards = await userService.GetHistory();

        return View(boards);
    }

    public async Task<IActionResult> Details(int Id)
    {
        UserBoardsViewModel board = await userService.BoardDetails(Id);

        return View(board);
    }
}
