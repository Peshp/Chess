namespace Chess.Web.ViewModels.User;

using System.Collections.Generic;

using global::Chess.Web.ViewModels.Contracts;

using ViewModels.Chess;

public class UserBoardsViewModel : IBoardViewModel
{
    public int Id { get; set; }

    public string UserId { get; set; }

    public int BoardId { get; set; }

    public string Image { get; set; }

    public string Date { get; set; }

    public IList<SquareViewModel> MoveHistory { get; set; } = new List<SquareViewModel>();

    public IList<FigureViewModel> Figures { get; set; } = new List<FigureViewModel>();
}
