namespace Chess.Services.Helpers
{
    using System.Text;

    using Chess.Data.Models;
    using Chess.Web.ViewModels.Chess;

    public static class FenHelper
    {
        public static string Generate(BoardViewModel board, string activeColor)
        {
            StringBuilder fen = new StringBuilder();

            for (double y = 0; y <= 87.5; y += 12.5)
            {
                int emptySquares = 0;
                for (double x = 0; x <= 87.5; x += 12.5)
                {
                    var figure = board.Figures.FirstOrDefault(f =>
                        Math.Abs(f.PositionX - x) < 0.1 &&
                        Math.Abs(f.PositionY - y) < 0.1);

                    if (figure == null)
                    {
                        emptySquares++;
                    }
                    else
                    {
                        if (emptySquares > 0)
                        {
                            fen.Append(emptySquares);
                            emptySquares = 0;
                        }
                        fen.Append(GetPieceChar(figure));
                    }
                }

                if (emptySquares > 0)
                {
                    fen.Append(emptySquares);
                }

                if (y < 87.5)
                {
                    fen.Append("/");
                }
            }

            fen.Append($" {activeColor.ToLower()} KQkq - 0 1");

            return fen.ToString();
        }

        private static char GetPieceChar(FigureViewModel figure)
        {
            // Get the character based on the name (e.g., "Pawn" -> 'p')
            char piece = figure.Name.ToLower() switch
            {
                "pawn" => 'p',
                "rook" => 'r',
                "knight" => 'n',
                "bishop" => 'b',
                "queen" => 'q',
                "king" => 'k',
                _ => ' '
            };

            return figure.Color == "White" ? char.ToUpper(piece) : piece;
        }
    }
}
