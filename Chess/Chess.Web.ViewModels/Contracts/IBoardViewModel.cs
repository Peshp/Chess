namespace Chess.Web.ViewModels.Contracts
{

    using ViewModels.Chess;


    public interface IBoardViewModel
    {
        int Id { get; set; }

        string UserId { get; set; }

        string Image { get; set; }

        IEnumerable<SquareViewModel> MoveHistory { get; set; }

        IEnumerable<FigureViewModel> Figures { get; set; }
    }
}
