namespace Chess.Domain.ViewModels.Web
{
    public class BoardViewModel
    {
        public string BoardImage { get; set; } = string.Empty;

        public List<FigureViewModel> Figures { get; set; } = new List<FigureViewModel>();
    }
}
