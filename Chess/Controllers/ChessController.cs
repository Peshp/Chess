namespace Chess.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Chess.Application.Interfaces;

    public class ChessController : Controller
    {
        private readonly IChessService chessService;

        public IActionResult Game()
        {
            return View();
        }
    }
}
