namespace Chess.Domain.ViewModels.Web;

using System.Text.Json.Serialization;

public class BoardViewModel
{
    public List<FigureViewModel> Figures { get; set; } = new();

    public List<FigureViewModel> CapturedFigures { get; set; } = new();

    public string BoardImage { get; set; }
}
