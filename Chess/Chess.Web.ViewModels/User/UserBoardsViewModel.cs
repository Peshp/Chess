namespace Chess.Web.ViewModels.User;

using System.Collections.Generic;

using ViewModels.Chess;

public class UserBoardsViewModel
{
    public int Id { get; set; }

    public string UserId { get; set; }

    public int BoardId { get; set; }

    public string Image { get; set; }

    public IEnumerable<SquareViewModel> MoveHistory { get; set; }

    public IEnumerable<FigureViewModel> Figures { get; set; }
}
