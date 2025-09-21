namespace Chess.Web.Controllers
{
    using Chess.Services.Data.Models;
    using Chess.Services.Data.Services.Contracts;
    using Chess.Services.Services;
    using Chess.Web.ViewModels.Chess;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class GameController : BaseController
    {
        private readonly IGameService _gameService;
        private readonly IEngineService engineService;
        private readonly ICheckService checkService;
        private readonly ICastleService castleService;

        public GameController
            (IGameService gameService, ICheckService checkService,
            ICastleService castleService, IEngineService engineService)
        {
            _gameService = gameService ?? throw new ArgumentNullException(nameof(gameService));
            this.engineService = engineService;
            this.checkService = checkService;
            this.castleService = castleService;
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
        public async Task<IActionResult> MakeMove([FromBody] Move request)
        {
            var board = HttpContext.Session.GetBoard();
            if (board == null)
                return Json(new { success = false});

            var piece = await engineService.FindPieceById(board, request.PieceId);
            if (piece == null)
                return Json(new { success = false });

            double toX = request.ToX * 12.5;
            double toY = request.ToY * 12.5;

            bool moveResult = false;

            if (piece.Name == "King")
            {
                moveResult = await castleService.Castle(board, piece, toX, toY);
            }
            else
            {
                moveResult = await engineService.TryMove(board, piece.Id, toX, toY);
            }

            bool isCheck = false;
            if (moveResult)
            {
                HttpContext.Session.SetBoard(board);
                if(piece.Name == "King")
                {
                    isCheck = await this.checkService.IsCheck(board, board.CurrentTurn);
                }
            }

            return Json(new
            {
                moveResult,
                isCheck,
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
}
