namespace Chess.Web.Controllers;

using System;
using System.Linq;
using System.Threading.Tasks;

using Chess.Services.Helpers;
using Chess.Services.Requests;
using Chess.Services.Services.Contracts;
using Chess.Web.Infrastructure.Extension;
using Chess.Web.ViewModels.Chess;

using Humanizer;

using Microsoft.AspNetCore.Mvc;

using static Chess.Services.Helpers.ParseUciMove;

public class GameController : BaseController
{
    private readonly IEngineService engineService;
    private readonly IMoveService moveService;
    private readonly ICheckService checkService;
    private readonly ICastleService castleService;
    private readonly IGameService gameService;
    private readonly IStockfishService stockfishService;

    public GameController(
        IEngineService engineService,
        IMoveService moveService,
        ICheckService checkService,
        ICastleService castleService,
        IGameService gameService,
        IStockfishService stockfishService)
    {
        this.engineService = engineService;
        this.moveService = moveService;
        this.checkService = checkService;
        this.castleService = castleService;
        this.gameService = gameService;
        this.stockfishService = stockfishService;
    }

    [HttpGet]
    public async Task<IActionResult> Game(ClockViewModel clock, string gameType)
    {
        string userId = User.GetId() ?? string.Empty;
        HttpContext.Session.Clear();
        BoardViewModel board = HttpContext.Session.GetBoard<BoardViewModel>();

        if (board == null)
        {
            board = await gameService.GetBoard(clock, userId);
            board.GameType = gameType;
            HttpContext.Session.SetBoard(board);
        }

        return View(board);
    }

    [HttpPost]
    public async Task<IActionResult> MakeMove([FromBody] Move request)
    {
        var board = HttpContext.Session.GetBoard<BoardViewModel>();
        if (board == null) return BadRequest();

        // Check if game is over due to time running out
        if (request.isGameOver)
        {
            board.IsGameOver = true;
            HttpContext.Session.SetBoard(board);
            
            return Json(new
            {
                success = false,
                isCheck = board.IsCheck,
                gameOver = board.IsGameOver,
                currentTurn = board.CurrentTurn,
                figures = board.FiguresJson,  
                captured = board.CapturedJson,
                moveHistory = board.HistoryJson
            });
        }

        board.Success = await engineService.TryMove(board, request.PieceId, request.ToX, request.ToY);

        if (board.Success)
        {
            await gameService.AddtoMoveHistory(board, request.PieceId, request.ToX, request.ToY);

            if (board.GameType == "AI" && !board.IsGameOver)
            {
                string activeColor = board.CurrentTurn == "White" ? "w" : "b";
                string fen = FenCoordinatesConverter.Generate(board, activeColor);

                string moveUci = await stockfishService.GetBestMoveAsync(fen, 10);

                if (!string.IsNullOrEmpty(moveUci))
                {
                    var aiMove = FromUci(moveUci, board);
                    if (aiMove.PieceId != null)
                    {
                        await engineService.TryMove(board, int.Parse(aiMove.PieceId), aiMove.ToX, aiMove.ToY);
                        await gameService.AddtoMoveHistory(board, int.Parse(aiMove.PieceId), aiMove.ToX, aiMove.ToY);
                    }
                }
            }

            board.IsCheck = await checkService.IsCheck(board, board.CurrentTurn);
            
            // Only check for checkmate if game is not already over due to timeout
            if (!board.IsGameOver)
            {
                board.IsGameOver = await engineService.IsCheckmate(board, board.CurrentTurn);
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
        string userId = User.GetId();

        BoardViewModel board = this.HttpContext.Session.GetBoard<BoardViewModel>();
        gameService.SaveBoard(board, userId);

        return RedirectToAction("Index", "Home");
    }
}
