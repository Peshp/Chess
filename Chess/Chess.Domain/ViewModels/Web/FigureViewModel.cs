namespace Chess.Domain.ViewModels.Web
{
    public class FigureViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Image { get; set; } = string.Empty;

        public string Color { get; set; } = string.Empty;

        public int PositionX { get; set; }

        public int PositionY { get; set; }
    }
}
