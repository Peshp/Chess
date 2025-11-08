namespace Chess.Web.Controllers;

using System;
using System.Linq;
using System.Threading.Tasks;

using Chess.Web.ViewModels.Chess;

using Microsoft.AspNetCore.Mvc;
using Chess.Web.Infrastructure.Extension;
using Chess.Services.Services.Contracts;
using Chess.Services.Models;
using Chess.Services;

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
        BoardViewModel board = HttpContext.Session.GetBoard();

        if(User.Identity.IsAuthenticated)
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
        var board = this.HttpContext.Session.GetBoard();
        if (board == null)
            return Json(new { success = false });

        double toX = request.ToX * 12.5;
        double toY = request.ToY * 12.5;

        bool success = await engineService.TryMove(board, request.PieceId, toX, toY);

        bool isCheck = false;
        bool gameOver = false;

        if (success)
        {
            await gameService.AddtoMoveHistory(board, request.PieceId, toX, toY);

            HttpContext.Session.SetBoard(board);

            isCheck = await checkService.IsCheck(board, board.CurrentTurn);
            gameOver = await engineService.IsCheckmate(board, board.CurrentTurn);
        }

        return Json(new
        {
            success,
            isCheck,
            gameOver,
            currentTurn = board.CurrentTurn,
            figures = board.Figures.Select(f => new
            {
                id = f.Id,
                x = f.PositionX,
                y = f.PositionY,
                name = f.Name,
                color = f.Color,
                image = f.Image,
                isMoved = f.IsMoved,
            }),
            captured = board.CapturedFigures.Select(f => new
            {
                color = f.Color,
                image = f.Image,
            }),
            moveHistory = board.MoveHistory.Select(m => new
            {
                coordinate = m.Coordinate,
                figureImage = m.FigureImage,
            }),
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

        BoardViewModel board = this.HttpContext.Session.GetBoard();
        gameService.SaveBoard(board, userId);

        return View();
    }
}
