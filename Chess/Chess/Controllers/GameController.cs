namespace Chess.Controllers;

using Microsoft.AspNetCore.Mvc;
using Application.interfaces;
using Presentation.Extensions;
using Presentation.Requests;
using Presentation.ViewModels.Web;

public class GameController : Controller
{
    private readonly IGameService _gameService;

    public GameController(IGameService gameService)
    {
        _gameService = gameService ?? throw new ArgumentNullException(nameof(gameService));
    }

    public async Task<IActionResult> Game()
    {
        BoardViewModel board = HttpContext.Session.GetBoard();
        if (board == null)
        {
            board = await _gameService.GetBoard();
            HttpContext.Session.SetBoard(board);
        }
        return View(board);
    }

    [HttpPost]
    public async Task<IActionResult> MakeMove([FromBody] MoveRequest request)
    {
        var board = HttpContext.Session.GetBoard();
        if (board == null)
            return Json(new { success = false });

        bool success = await _gameService.TryMove(board, request.pieceId, request.ToX * 12.5, request.ToY * 12.5);

        bool isCheck = false;
        if (success)
        {
            HttpContext.Session.SetBoard(board);
            isCheck = await _gameService.IsCheck(board, board.CurrentTurn);
        }

        return Json(new
        {
            success,
            isCheck, // frontend can use this!
            currentTurn = board.CurrentTurn,
            figures = board.Figures.Select(f => new {
                id = f.Id,
                x = f.PositionX,
                y = f.PositionY,
                name = f.Name,
                color = f.Color, 
                image = f.Image,
                isMoved = f.IsMoved
            }),
            captured = board.CapturedFigures.Select(f => new {
                id = f.Id,
                name = f.Name,
                color = f.Color,
                image = f.Image
            }),
        });
    }

    //[HttpPost]
    //public async Task<IActionResult> EndGame()
    //{
    //    var board = HttpContext.Session.GetBoard();
    //    if (board != null)
    //    {
    //        // TODO: Implement SaveBoard in GameService to persist to DB
    //        await _gameService.SaveBoard(board);
    //        HttpContext.Session.Remove("Board");
    //    }
    //    return Json(new { success = true });
    //}

}
