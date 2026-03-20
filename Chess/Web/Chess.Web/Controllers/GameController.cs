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
using Microsoft.Extensions.Caching.Distributed;

using static Chess.Services.Helpers.ParseUciMove;

// .NET 10 Primary Constructor: Clean, concise injection
public class GameController(
    IDistributedCache _cache,
    IEngineService engineService,
    IGameService gameService,
    StockfishService stockfishService) : BaseController
{

    [HttpGet]
    public async Task<IActionResult> Game(ClockViewModel clock, string gameType)
    {
        string userId = User.GetId();

        var board = await gameService.GetBoard(clock, userId);
        board.GameType = gameType;

        await _cache.SetRecordAsync($"board.{userId}", board, TimeSpan.FromHours(1));

        return View(board);
    }

    [HttpPost]
    public async Task<IActionResult> MakeMove([FromBody] Move request)
    {
        string userId = User?.GetId();
        var board = await _cache.GetRecordAsync<BoardViewModel>($"board.{userId}");

        board.Success = await engineService.TryMove(board, request.PieceId, request.ToX, request.ToY);

        if (board.Success)
        {
            var movedPiece = board.Figures.FirstOrDefault(f => f.Id == request.PieceId);

            if (await engineService.PawnOnEdge(board, request.PieceId))
            {
                await _cache.SetRecordAsync($"board.{userId}", board, TimeSpan.FromHours(1));
                return Json(new
                {
                    success = true,
                    needsPromotion = true,
                    pieceId = movedPiece.Id,
                    currentTurn = board.CurrentTurn,
                    figures = board.FiguresJson,
                    captured = board.CapturedJson,
                    moveHistory = board.HistoryJson
                });
            }
            
            await gameService.AddtoMoveHistory(board, request.PieceId, request.ToX, request.ToY);

            if (board.GameType == "AI" && !board.IsGameOver)
            {
                string activeColor = board.CurrentTurn == "White" ? "w" : "b";
                string fen = FenCoordinatesConverter.Generate(board, activeColor);

                string moveUci = await stockfishService.GetBestMoveAsync(fen);

                if (!string.IsNullOrEmpty(moveUci))
                {
                    var aiMove = ParseUciMove.FromUci(moveUci, board);
                    if (aiMove.PieceId != null)
                    {
                        await engineService.TryMove(board, int.Parse(aiMove.PieceId), aiMove.ToX, aiMove.ToY);
                        await gameService.AddtoMoveHistory(board, int.Parse(aiMove.PieceId), aiMove.ToX, aiMove.ToY);
                    }
                }
            }

            board.IsCheck = await engineService.IsCheck(board, board.CurrentTurn);
            board.IsGameOver = await engineService.IsCheckmate(board, board.CurrentTurn, User.GetId());

            await _cache.SetRecordAsync($"board.{userId}", board, TimeSpan.FromHours(1));
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

    [HttpPost]
    public async Task<IActionResult> PromotePawn([FromBody] PromotionRequest request)
    {
        string userId = User.GetId();
        var board = await _cache.GetRecordAsync<BoardViewModel>($"board.{userId}");

        var pawn = board.Figures.FirstOrDefault(f => f.Id == request.PieceId);

        if (!new[] { "Queen", "Rook", "Bishop", "Night" }.Contains(request.PromoteTo))
            return BadRequest("Invalid promotion piece");

        pawn.Name = request.PromoteTo;
        pawn.Image = $"{char.ToLower(pawn.Color[0])}{request.PromoteTo[0]}.png";

        await gameService.AddtoMoveHistory(board, request.PieceId, pawn.PositionX, pawn.PositionY);

        board.IsCheck = await engineService.IsCheck(board, board.CurrentTurn);
        board.IsGameOver = await engineService.IsCheckmate(board, board.CurrentTurn, User.GetId());

        await _cache.SetRecordAsync($"board.{userId}", board, TimeSpan.FromHours(1));

        return Json(new
        {
            success = true,
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
        var board = await _cache.GetRecordAsync<BoardViewModel>($"board.{userId}");

        if (board != null && !string.IsNullOrEmpty(userId))
        {
            await gameService.SaveBoard(board, userId);
            await _cache.RemoveAsync($"board.{userId}"); 
        }

        return RedirectToAction("Index", "Home");
    }
}
