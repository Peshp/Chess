namespace Chess.Web.Controllers;

using System;
using System.Linq;
using System.Threading.Tasks;

using Chess.Web.ViewModels.Chess;

using Microsoft.AspNetCore.Mvc;
using Chess.Web.Infrastructure.Extension;
using static Chess.Web.Infrastructure.Extension.CurrentSessionExtension;
using static Chess.Services.Helpers.ParseUciMoveHelper;
using Chess.Services.Services.Contracts;
using Chess.Services;
using Chess.Web.ViewModels.User;
using Chess.Services.Helpers;

public class GameController : BaseController
{
    private readonly IEngineService engineService;
    private readonly IGameService gameService;
    private readonly IStockfishService stockfishService;

    public GameController(
        IEngineService engineService,
        IGameService gameService,
        IStockfishService stockfishService)
    {
        this.engineService = engineService;
        this.gameService = gameService;
        this.stockfishService = stockfishService;
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
        var board = HttpContext.Session.GetBoard<BoardViewModel>();

        double toX = request.ToX * 12.5;
        double toY = request.ToY * 12.5;

        board.Success = await engineService.TryMove(board, request.PieceId, toX, toY);

        if (board.Success)
        {
            await gameService.AddtoMoveHistory(board, request.PieceId, toX, toY);

            if (!board.IsGameOver)
            {
                string aiActiveColor = board.CurrentTurn == "White" ? "w" : "b";
                string fen = FenHelper.Generate(board, aiActiveColor);

                string aiMoveUci = await stockfishService.GetBestMoveAsync(fen, 10);

                if (!string.IsNullOrEmpty(aiMoveUci))
                {
                    var aiMove = ParseUciMove(aiMoveUci, board);

                    if (aiMove.PieceId != null)
                    {
                        await engineService.TryMove(board, int.Parse(aiMove.PieceId), aiMove.ToX, aiMove.ToY);
                        await gameService.AddtoMoveHistory(board, int.Parse(aiMove.PieceId), aiMove.ToX, aiMove.ToY);
                    }
                }
            }

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
