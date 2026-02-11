namespace Chess.Web.Infrastructure.Extension;

using System.Text.Json;

using Chess.Web.ViewModels.Chess;
using Chess.Web.ViewModels.Contracts;

using Microsoft.AspNetCore.Http;

public static class CurrentSessionExtension
{
    public static void SetBoard<T>(this ISession session, T board) where T : IBoardViewModel
    {
        var bytes = JsonSerializer.SerializeToUtf8Bytes(board);
        session.Set("Board", bytes);
    }

    public static T GetBoard<T>(this ISession session) where T : class, IBoardViewModel
    {
        if (session.TryGetValue("Board", out var bytes))
        {
            return JsonSerializer.Deserialize<T>(bytes);
        }

        return null;
    }
}
