namespace Chess.Data.Models;

public class BoardSquares
{
    public int Id { get; set; }

    public int FigureId { get; set; }

    public string Coordinates { get; set; } = string.Empty;

    public double PositionX { get; set; }

    public double PositionY { get; set; }

    public string FigureImage { get; set; }

    public int BoardId { get; set; }

    public UserBoard Board { get; set; }
}
