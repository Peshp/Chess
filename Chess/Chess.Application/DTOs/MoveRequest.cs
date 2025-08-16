namespace Chess.Application.DTOs
{
    using Domain.ViewModels.Web;

    public class MoveRequest
    {
        public int pieceId { get; set; }

        public double ToX { get; set; }

        public double ToY { get; set; }

        public BoardViewModel board { get; set; }

    }
}
