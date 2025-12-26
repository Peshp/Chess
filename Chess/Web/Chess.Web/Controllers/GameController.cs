namespace Chess.Web.Controllers;

using System;
using System.Linq;
using System.Threading.Tasks;

using Chess.Web.ViewModels.Chess;

using Microsoft.AspNetCore.Mvc;
using Chess.Web.Infrastructure.Extension;
using static Chess.Web.Infrastructure.Extension.CurrentSessionExtension;
using Chess.Services.Services.Contracts;
using Chess.Services;
using Chess.Web.ViewModels.User;

public class GameController : BaseController
{
    private readonly IEngineService engineService;
    private readonly IMoveService moveService;
    private readonly ICheckService checkService;
    private readonly ICastleService castleService;
    private readonly IGameService gameService;

    public GameController(
        IEngineService engineService,
        IMoveService moveService,
        ICheckService checkService,
        ICastleService castleService,
        IGameService gameService)
    {
        this.engineService = engineService;
        this.moveService = moveService;
        this.checkService = checkService;
        this.castleService = castleService;
        this.gameService = gameService;
    }

    public async Task<IActionResult> Game(ClockViewModel clock)
    {
        string userId = string.Empty;
        HttpContext.Session.Clear();
        BoardViewModel board = HttpContext.Session.GetBoard<BoardViewModel>();

        if (User?.Identity?.IsAuthenticated == true)
        {
            userId = User.GetId();
        }

        if (board == null)
        {
            board = await gameService.GetBoard(clock, userId);
            HttpContext.Session.SetBoard(board);
        }

        return View(board);
    }

    [HttpPost]
    public async Task<IActionResult> MakeMove([FromBody] Move request)
    {
        var board = this.HttpContext.Session.GetBoard<BoardViewModel>();
        if (board == null)
            return Json(new { success = false });

        double toX = request.ToX * 12.5;
        double toY = request.ToY * 12.5;

        board.Success = await engineService.TryMove(board, request.PieceId, toX, toY);

        if (board.Success)
        {
            await gameService.AddtoMoveHistory(board, request.PieceId, toX, toY);

            HttpContext.Session.SetBoard(board);
        }

        return Json(new
        {
            success = board.Success,
            isCheck = board.IsCheck,
            gameOver = board.IsGameOver,
            currentTurn = board.CurrentTurn,
            figures = board.FiguresJson,
            captured = board.CapturedJson,
            moveHistory = board.HistoryJson
        });
    }

    [HttpGet]
    public async Task<IActionResult> EndGame()
    {
        string userId = string.Empty;

        if (User.Identity.IsAuthenticated)
        {
            userId = User.GetId();
        }

        if(userId == string.Empty)
        {
            return RedirectToAction("Index", "Home");
        }

        BoardViewModel board = this.HttpContext.Session.GetBoard<BoardViewModel>();
        gameService.SaveBoard(board, userId);

        return RedirectToAction("Index", "Home");
    }
}
