namespace Chess.Web.Controllers;

using System;
using System.Linq;
using System.Threading.Tasks;

using Chess.Services.Helpers;
using Chess.Services.Requests;
using Chess.Services.Services;
using Chess.Services.Services.Contracts;
using Chess.Web.Infrastructure.Extension;
using Chess.Web.ViewModels.Chess;

using Microsoft.AspNetCore.Mvc;

using static Chess.Services.Helpers.ParseUciMove;

// .NET 10 Primary Constructor: Clean, concise injection
public class GameController(
    IEngineService engineService,
    IMoveService moveService,
    ICheckService checkService,
    IGameService gameService,
    StockfishService stockfishService) : BaseController
{

    [HttpGet]
    public async Task<IActionResult> Game(ClockViewModel clock, string gameType)
    {
        string userId = User.GetId() ?? string.Empty;

        var board = HttpContext.Session.GetBoard<BoardViewModel>();

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

        board.Success = await engineService.TryMove(board, request.PieceId, request.ToX, request.ToY);

        if (board.Success)
        {
            await gameService.AddtoMoveHistory(board, request.PieceId, request.ToX, request.ToY);

            if (board.GameType == "AI" && !board.IsGameOver)
            {
                string activeColor = board.CurrentTurn == "White" ? "w" : "b";
                string fen = FenCoordinatesConverter.Generate(board, activeColor);

                // This line calls the background service and waits for the AI move
                string moveUci = await stockfishService.GetBestMoveAsync(fen);

                if (!string.IsNullOrEmpty(moveUci))
                {
                    var aiMove = ParseUciMove.FromUci(moveUci, board);
                    if (aiMove.PieceId != null)
                    {
                        // Apply AI move to the board
                        await engineService.TryMove(board, int.Parse(aiMove.PieceId), aiMove.ToX, aiMove.ToY);
                        await gameService.AddtoMoveHistory(board, int.Parse(aiMove.PieceId), aiMove.ToX, aiMove.ToY);
                    }
                }
            }

            board.IsCheck = await checkService.IsCheck(board, board.CurrentTurn);
            board.IsGameOver = await engineService.IsCheckmate(board, board.CurrentTurn);

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
        string userId = User.GetId() ?? string.Empty;
        var board = HttpContext.Session.GetBoard<BoardViewModel>();

        if (board != null)
        {
            await gameService.SaveBoard(board, userId);
            HttpContext.Session.Remove("Board"); 
        }

        return RedirectToAction("Index", "Home");
    }
}
