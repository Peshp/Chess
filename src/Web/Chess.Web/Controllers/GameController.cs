namespace Chess.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Chess.Services.Data.Models;
    using Chess.Services.Data.Services.Contracts;
    using Chess.Services.Services;
    using Chess.Web.ViewModels.Chess;

    using Microsoft.AspNetCore.Mvc;

    public class GameController : BaseController
    {
        private readonly IGameService gameService;
        private readonly IEngineService engineService;

        public GameController(IGameService gameService, IEngineService engineService)
        {
            this.gameService = gameService ?? throw new ArgumentNullException(nameof(gameService));
            this.engineService = engineService ?? throw new ArgumentNullException(nameof(engineService));
        }

        public async Task<IActionResult> Game()
        {
            BoardViewModel board = HttpContext.Session.GetBoard();
            if (board == null)
            {
                board = await gameService.GetBoard();
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
            bool GameOver = false;
            if (success)
            {
                await gameService.AddtoMoveHistory(board, request.PieceId, toX, toY);

                HttpContext.Session.SetBoard(board);
                isCheck = await engineService.IsCheck(board, board.CurrentTurn);
                GameOver = await engineService.IsCheckmate(board, board.CurrentTurn);
            }

            return Json(new
            {
                success,
                isCheck,
                GameOver,
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

        public async Task<IActionResult> EndGame()
        {
            BoardViewModel board = this.HttpContext.Session.GetBoard();
            await gameService.SaveBoard(board);

            return View();
        }
    }
}
