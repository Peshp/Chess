namespace Chess.Application.DTOs
{
    public class MoveRequest
    {
        public int PieceId { get; set; }

        public int ToX { get; set; }

        public int ToY { get; set; }

    }
}
