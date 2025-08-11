namespace Chess.Controllers
{
    using Application.interfaces;
    using Application.DTOs;
    using Domain.ViewModels.Web;
    using Microsoft.AspNetCore.Mvc;

    public class GameController : Controller
    {
        private readonly IGameService _gameService;

        public GameController(IGameService gameService)
        {
            _gameService = gameService ?? throw new ArgumentNullException(nameof(gameService));
        }

        public async Task<IActionResult> Game()
        {
            BoardViewModel board = await _gameService.GetBoard();

            return View(board);
        }

        [HttpPost]
        public async Task<IActionResult> MakeMove([FromBody] MoveRequest request)
        {
            bool success = await _gameService.TryMove(request.pieceId, request.ToX * 12.5, request.ToY * 12.5);
            return Json(new { success });
        }

    }
}
