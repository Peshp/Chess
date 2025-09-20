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
        private readonly ICheckService checkService;
        private readonly ICastleService castleService;

        public GameController(IGameService gameService, ICheckService checkService, ICastleService castleService)
        {
            this.gameService = gameService ?? throw new ArgumentNullException(nameof(gameService));
            this.checkService = checkService ?? throw new ArgumentNullException(nameof(checkService));
            this.castleService = castleService ?? throw new ArgumentNullException(nameof(castleService));
        }

        public async Task<IActionResult> Game()
        {
            BoardViewModel board = HttpContext.Session.GetBoard();
            if (board == null)
            {
                board = await this.gameService.GetBoard();
                HttpContext.Session.SetBoard(board);
            }
            return View(board);
        }

        [HttpPost]
        public async Task<IActionResult> MakeMove([FromBody] Move request)
        {
            var board = HttpContext.Session.GetBoard();

            var piece = board.Figures.FirstOrDefault(f => f.Id == request.PieceId);

            double toX = request.ToX * 12.5;
            double toY = request.ToY * 12.5;

            if (!await this.checkService.IsLegalMove(board, piece, toX, toY))
            {
                return Json(new { success = false });
            }

            bool result = false;

            if (piece.Name == "King" && this.castleService.IsCastleAttempt(piece, toX, toY))
            {
                if (await this.castleService.IsCastleLegal(board, piece, toX, toY))
                {
                    this.castleService.PerformCastleMove(piece, board, toX, toY);
                    board.CurrentTurn = (board.CurrentTurn == "White") ? "Black" : "White";
                    result = true;
                }
                else
                {
                    return Json(new { success = false, message = "Illegal castle move." });
                }
            }
            else
            {
                if (board.CurrentTurn != piece.Color)
                    return Json(new { success = false, message = "Not your turn." });

                result = await this.gameService.TryMove(board, request.PieceId, toX, toY);
            }

            bool isCheck = false;
            bool hasEscape = true;
            if (result)
            {
                HttpContext.Session.SetBoard(board);

                string opponentColor = (board.CurrentTurn == "White") ? "Black" : "White";
                isCheck = await this.checkService.IsCheck(board, opponentColor);
                hasEscape = await this.checkService.HasLegalMoveToEscapeCheck(board, opponentColor);
            }

            return Json(new
            {
                success = result,
                isCheck,
                hasEscape,
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
