namespace Chess.Web.ViewModels.Contracts
{

    using ViewModels.Chess;


    public interface IBoardViewModel
    {
        int Id { get; set; }

        string UserId { get; set; }

        string Image { get; set; }

        public IEnumerable<SquareViewModel> MoveHistory { get; set; }

        public IEnumerable<FigureViewModel> Figures { get; set; }
    }
}
