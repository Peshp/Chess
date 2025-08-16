namespace Chess.Domain.ViewModels.Web
{
    using System.Text.Json.Serialization;

    public class BoardViewModel
    {
        public List<FigureViewModel> Figures { get; set; }

        public string BoardImage { get; set; }
    }

}
