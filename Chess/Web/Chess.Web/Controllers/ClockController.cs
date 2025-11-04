namespace Chess.Web.Controllers;

using System.Threading.Tasks;

using Chess.Web.ViewModels.Chess;
using Microsoft.AspNetCore.Mvc;

public class ClockController
{
    public IActionResult Clock(ClockViewModel model)
    {
        return new RedirectToActionResult("Game", "Game", new { minutes = model.Minutes, increment = model.Increment });
    }
}
