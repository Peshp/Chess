namespace Chess.Web.ViewModels.Contracts
{

    using ViewModels.Chess;


    public interface IBoardViewModel
    {
        int Id { get; set; }

        string UserId { get; set; }

        string Image { get; set; }

        public IList<SquareViewModel> MoveHistory { get; set; }

        public IList<FigureViewModel> Figures { get; set; }
    }
}
