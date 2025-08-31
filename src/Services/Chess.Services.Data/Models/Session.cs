namespace Chess.Services.Data.Models
{
    using System.Text.Json;

    using Chess.Web.ViewModels.Chess;

    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// Provides extension methods for managing chess board data in an ISession.
    /// </summary>
    public static class Session
    {
        /// <summary>
        /// Saves the chess board state to the session.
        /// </summary>
        /// <param name="session">The session instance.</param>
        /// <param name="board">The chess board data to save.</param>
        public static void SetBoard(this ISession session, BoardViewModel board)
        {
            var bytes = JsonSerializer.SerializeToUtf8Bytes(board);
            session.Set("Board", bytes);
        }

        /// <summary>
        /// Retrieves the chess board state from the session.
        /// </summary>
        /// <param name="session">The session instance.</param>
        /// <returns>The chess board data, or null if not found.</returns>
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
