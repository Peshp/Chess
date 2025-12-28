namespace Chess.Services.Services.Contracts
{
    public interface IStockfishService
    {
        Task<string> GetBestMoveAsync(string fen, int skillLevel);
    }
}
