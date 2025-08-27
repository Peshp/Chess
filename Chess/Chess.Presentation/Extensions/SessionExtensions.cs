namespace Chess.Presentation.Extensions;

using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Presentation.ViewModels.Web;

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
