namespace Chess.Domain.Extensions
{
    using Domain.ViewModels.Web;
    using Microsoft.AspNetCore.Http;
    using System.Text;
    using System.Text.Json;

    public static class SessionExtensions
    {
        public static void SetBoard(this ISession session, BoardViewModel board)
        {
            var bytes = JsonSerializer.SerializeToUtf8Bytes(board);
            session.Set("Board", bytes);
        }

        public static BoardViewModel GetBoard(this ISession session)
        {
            if (session.TryGetValue("Board", out var bytes))
            {
                return JsonSerializer.Deserialize<BoardViewModel>(bytes);
            }
            return null;
        }
    }
}
