using Chess.Application.Services;
using Chess.Contracts;
using Chess.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Chess.Web.Controllers
{
    public class GameController : Controller
    {
        private readonly GameService _gameService;

        // For demo, keep castling/en passant in TempData. 
        // In production, use a proper persistent store or database!
        private const string CastlingRightsKey = "CastlingRights";
        private const string EnPassantTargetKey = "EnPassantTarget";

        public GameController()
        {
            _gameService = new GameService();
        }

        // GET: /Chess/Board
        public IActionResult Board()
        {
            // For example, load a new board or from database/session
            var viewModel = new ChessBoardViewModel
            {
                Figures = ChessStarterBoard(), // You must implement ChessStarterBoard() to set up pieces
                CurrentTurn = "White",
                // Other fields as needed...
            };

            // Example: store castling rights/en passant initial state in TempData/Session
            TempData[CastlingRightsKey] = Newtonsoft.Json.JsonConvert.SerializeObject(new GameService.CastlingRights());
            TempData[EnPassantTargetKey] = null;

            return View(viewModel);
        }

        // POST: /Chess/Move
        [HttpPost]
        public IActionResult Move(int fromRow, int fromCol, int toRow, int toCol)
        {
            // Load board, rights, en passant from session/db
            var viewModel = LoadViewModel(); // Implement this per your storage
            var castlingRights = TempData[CastlingRightsKey] != null
                ? Newtonsoft.Json.JsonConvert.DeserializeObject<GameService.CastlingRights>(TempData[CastlingRightsKey].ToString())
                : new GameService.CastlingRights();
            var enPassantTarget = TempData[EnPassantTargetKey] != null
                ? Newtonsoft.Json.JsonConvert.DeserializeObject<GameService.EnPassantTarget>(TempData[EnPassantTargetKey].ToString())
                : null;

            string error;
            bool legal = _gameService.TryMove(
                viewModel.Figures, fromRow, fromCol, toRow, toCol,
                viewModel.CurrentTurn, castlingRights, ref enPassantTarget, out error);

            if (legal)
            {
                viewModel.MoveHistory.Add($"{viewModel.CurrentTurn}: ({fromRow},{fromCol}) to ({toRow},{toCol})");
                viewModel.CurrentTurn = (viewModel.CurrentTurn == "White") ? "Black" : "White";
                // Save state
                TempData[CastlingRightsKey] = Newtonsoft.Json.JsonConvert.SerializeObject(castlingRights);
                TempData[EnPassantTargetKey] = enPassantTarget != null ? Newtonsoft.Json.JsonConvert.SerializeObject(enPassantTarget) : null;
                SaveViewModel(viewModel); // Implement this
            }
            else
            {
                TempData["ChessMoveError"] = error ?? "Illegal move";
            }

            return RedirectToAction("Board");
        }

        // Helper methods (implement for your application)
        private ChessBoardViewModel LoadViewModel()
        {
            // Load board state from session/db, etc.
            // For demo, always return a new board (replace in real app)
            return new ChessBoardViewModel
            {
                Figures = ChessStarterBoard(),
                CurrentTurn = "White"
            };
        }

        private void SaveViewModel(ChessBoardViewModel vm)
        {
            // Save board state to session/db, etc.
        }

        private List<Figure> ChessStarterBoard()
        {
            // Return a list of Figure objects representing the initial chess setup
            // Implement this for your Figure/Board model
            return new List<Figure>
        {
            // Example for white pawns only:
            new Figure { Type = FigureType.Pawn, Row = 6, Col = 0, Color = "White" },
            new Figure { Type = FigureType.Pawn, Row = 6, Col = 1, Color = "White" },
            // ... all other pieces
        };
        }
    }
}
