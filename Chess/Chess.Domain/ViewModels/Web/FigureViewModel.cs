namespace Chess.Domain.ViewModels.Web;

using System.Text.Json.Serialization;

public class FigureViewModel
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Image { get; set; }

    public double PositionX { get; set; }

    public double PositionY { get; set; }

    public string Color { get; set; }

    public bool IsMoved { get; set; }
}
