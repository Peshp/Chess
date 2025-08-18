namespace Chess.Application.Requests;

public class MoveRequest
{
    public int pieceId { get; set; }

    public double ToX { get; set; }

    public double ToY { get; set; }

}
