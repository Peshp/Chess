namespace Chess.Services.Models;

using System.Text.Json;

using Chess.Web.ViewModels.Chess;

using Microsoft.AspNetCore.Http;

public static class CurrentSessionExtension
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
