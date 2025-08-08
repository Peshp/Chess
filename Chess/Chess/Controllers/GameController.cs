namespace Chess.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class GameController : Controller
    {
        public IActionResult Game()
        {
            return View();
        }
    }
}
