namespace Chess.Services.Requests;

public class PromotionRequest
{
    public int PieceId { get; set; }

    public string PromoteTo { get; set; } // "Queen", "Rook", "Bishop", "Knight"
}