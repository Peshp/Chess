namespace Chess.infrastructure.Extensions
{
    using Chess.infrastructure.Entities;

    public static class BoardStateCache
    {
        public static List<Figure> Figures { get; set; } = new List<Figure>();
    }
}
