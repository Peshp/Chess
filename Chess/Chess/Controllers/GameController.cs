namespace Chess.Controllers
{
    using Application.interfaces;
    using Chess.Domain.ViewModels.Web;
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
    }
}
