namespace Chess.Data.Models;
public class Square
{
    public int Id { get; set; }

    public double PositionX { get; set; }

    public double PositionY { get; set; }

    public string Coordinate { get; set; }

    public int BoardId { get; set; }

    public Board? Board { get; set; }
}
