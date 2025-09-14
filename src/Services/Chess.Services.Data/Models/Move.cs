namespace Chess.Services.Data.Models
{
    public record Move
    {
        public int PieceId { get; set; }

        public double ToX { get; set; }

        public double ToY { get; set; }
    }
}
