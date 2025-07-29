namespace Chess.Application.Services
{
    using Domain.Entities;

    public class GameService
    {
        public class CastlingRights
        {
            public CastlingSide White { get; set; } = new CastlingSide();

            public CastlingSide Black { get; set; } = new CastlingSide();
        }

        public class CastlingSide
        {
            public bool Short { get; set; } = true;

            public bool Long { get; set; } = true;
        }

        public class EnPassantTarget
        {
            public int Row { get; set; }

            public int Col { get; set; }
        }

        // --- Board Query ---

        public Figure? GetPieceAt(List<Figure> figures, int row, int col)
        {
            return figures.FirstOrDefault(f => f.Row == row && f.Col == col);
        }

        public bool IsWhiteSquare(int row, int col)
        {
            return (row + col) % 2 == 0;
        }

        // --- Piece Movement Legality ---

        public bool IsBishopMoveLegal(List<Figure> board, int fromRow, int fromCol, int toRow, int toCol, string bishopColor, string squareColor)
        {
            int rowDiff = toRow - fromRow;
            int colDiff = toCol - fromCol;
            if (Math.Abs(rowDiff) != Math.Abs(colDiff)) return false;
            if (bishopColor != squareColor) return false;
            int rowDirection = rowDiff > 0 ? 1 : -1;
            int colDirection = colDiff > 0 ? 1 : -1;
            int steps = Math.Abs(rowDiff);
            for (int i = 1; i < steps; i++)
            {
                int intermediateRow = fromRow + i * rowDirection;
                int intermediateCol = fromCol + i * colDirection;
                if (GetPieceAt(board, intermediateRow, intermediateCol) != null) return false;
            }
            return true;
        }

        public bool IsRookMoveLegal(List<Figure> board, int fromRow, int fromCol, int toRow, int toCol)
        {
            if (!(fromRow == toRow || fromCol == toCol)) return false;
            int rowDirection = (toRow == fromRow) ? 0 : (toRow > fromRow ? 1 : -1);
            int colDirection = (toCol == fromCol) ? 0 : (toCol > fromCol ? 1 : -1);
            int steps = Math.Max(Math.Abs(toRow - fromRow), Math.Abs(toCol - fromCol));
            for (int i = 1; i < steps; i++)
            {
                int intermediateRow = fromRow + i * rowDirection;
                int intermediateCol = fromCol + i * colDirection;
                if (GetPieceAt(board, intermediateRow, intermediateCol) != null) return false;
            }
            return true;
        }

        public bool IsQueenMoveLegal(List<Figure> board, int fromRow, int fromCol, int toRow, int toCol)
        {
            if (Math.Abs(toRow - fromRow) == Math.Abs(toCol - fromCol))
            {
                int rowDirection = (toRow > fromRow) ? 1 : -1;
                int colDirection = (toCol > fromCol) ? 1 : -1;
                int steps = Math.Abs(toRow - fromRow);
                for (int i = 1; i < steps; i++)
                {
                    int intermediateRow = fromRow + i * rowDirection;
                    int intermediateCol = fromCol + i * colDirection;
                    if (GetPieceAt(board, intermediateRow, intermediateCol) != null) return false;
                }
                return true;
            }
            if (fromRow == toRow || fromCol == toCol)
            {
                return IsRookMoveLegal(board, fromRow, fromCol, toRow, toCol);
            }
            return false;
        }

        public bool IsKnightMoveLegal(List<Figure> board, int fromRow, int fromCol, int toRow, int toCol, string knightColor)
        {
            int rowDiff = Math.Abs(toRow - fromRow);
            int colDiff = Math.Abs(toCol - fromCol);
            if (!((rowDiff == 2 && colDiff == 1) || (rowDiff == 1 && colDiff == 2))) return false;
            var targetPiece = GetPieceAt(board, toRow, toCol);
            if (targetPiece != null && targetPiece.Color == knightColor) return false;
            return true;
        }

        public bool IsKingMoveLegal(List<Figure> board, int fromRow, int fromCol, int toRow, int toCol, string kingColor, CastlingRights castlingRights)
        {
            int rowDiff = Math.Abs(toRow - fromRow);
            int colDiff = Math.Abs(toCol - fromCol);
            if (rowDiff > 1 || colDiff > 1)
            {
                // Castling
                return IsCastlingMoveLegal(board, fromRow, fromCol, toRow, toCol, kingColor, castlingRights);
            }
            var targetPiece = GetPieceAt(board, toRow, toCol);
            if (targetPiece != null && targetPiece.Color == kingColor) return false;
            return !IsSquareAttacked(board, toRow, toCol, kingColor);
        }

        public bool IsSquareAttacked(List<Figure> board, int row, int col, string color)
        {
            string enemyColor = color == "White" ? "Black" : "White";
            foreach (var piece in board)
            {
                if (piece.Color != enemyColor) continue;
                string pieceType = piece.Type.ToString();
                int fromRow = piece.Row;
                int fromCol = piece.Col;
                if (pieceType == "Pawn")
                {
                    int direction = (enemyColor == "White") ? -1 : 1;
                    if (Math.Abs(col - fromCol) == 1 && (row - fromRow) == direction) return true;
                }
                if (pieceType == "Bishop")
                {
                    string fromSquareColor = IsWhiteSquare(fromRow, fromCol) ? "White" : "Black";
                    string toSquareColor = IsWhiteSquare(row, col) ? "White" : "Black";
                    if (IsBishopMoveLegal(board, fromRow, fromCol, row, col, fromSquareColor, toSquareColor)) return true;
                }
                if (pieceType == "Rook")
                {
                    if (IsRookMoveLegal(board, fromRow, fromCol, row, col)) return true;
                }
                if (pieceType == "Queen")
                {
                    if (IsQueenMoveLegal(board, fromRow, fromCol, row, col)) return true;
                }
                if (pieceType == "King")
                {
                    if (Math.Abs(row - fromRow) <= 1 && Math.Abs(col - fromCol) <= 1) return true;
                }
                if (pieceType == "Knight")
                {
                    if ((Math.Abs(row - fromRow) == 1 && Math.Abs(col - fromCol) == 2) ||
                        (Math.Abs(row - fromRow) == 2 && Math.Abs(col - fromCol) == 1)) return true;
                }
            }
            return false;
        }

        public Figure? FindKing(List<Figure> board, string color)
        {
            return board.FirstOrDefault(p => p.Type.ToString() == "King" && p.Color == color);
        }

        // --- Castling Logic ---

        public bool IsCastlingMoveLegal(List<Figure> board, int fromRow, int fromCol, int toRow, int toCol, string kingColor, CastlingRights castlingRights)
        {
            int baseRow = kingColor == "White" ? 7 : 0;
            if (fromRow != baseRow || toRow != baseRow) return false;
            if (fromCol != 4) return false;

            var king = GetPieceAt(board, fromRow, fromCol);
            if (king == null || king.Type.ToString() != "King" || king.Color != kingColor) return false;

            var rights = kingColor == "White" ? castlingRights.White : castlingRights.Black;

            if ((toCol == 6 && !rights.Short) || (toCol == 2 && !rights.Long)) return false;

            if (toCol == 6)
            {
                if (GetPieceAt(board, baseRow, 5) != null || GetPieceAt(board, baseRow, 6) != null) return false;
                var rook = GetPieceAt(board, baseRow, 7);
                if (rook == null || rook.Type.ToString() != "Rook" || rook.Color != kingColor) return false;
                if (IsSquareAttacked(board, fromRow, 4, kingColor) || IsSquareAttacked(board, baseRow, 5, kingColor) || IsSquareAttacked(board, baseRow, 6, kingColor)) return false;
                return true;
            }
            if (toCol == 2)
            {
                if (GetPieceAt(board, baseRow, 1) != null || GetPieceAt(board, baseRow, 2) != null || GetPieceAt(board, baseRow, 3) != null) return false;
                var rook = GetPieceAt(board, baseRow, 0);
                if (rook == null || rook.Type.ToString() != "Rook" || rook.Color != kingColor) return false;
                if (IsSquareAttacked(board, fromRow, 4, kingColor) || IsSquareAttacked(board, baseRow, 3, kingColor) || IsSquareAttacked(board, baseRow, 2, kingColor)) return false;
                return true;
            }
            return false;
        }

        // Moves the rook piece for castling and updates castling rights
        public void DoCastlingMove(List<Figure> board, int fromRow, int fromCol, int toCol, string kingColor, CastlingRights castlingRights)
        {
            int baseRow = kingColor == "White" ? 7 : 0;
            if (toCol == 6)
            {
                var rook = GetPieceAt(board, baseRow, 7);
                if (rook != null)
                {
                    rook.Col = 5;
                }
                var rights = kingColor == "White" ? castlingRights.White : castlingRights.Black;
                rights.Short = false;
                rights.Long = false;
            }
            if (toCol == 2)
            {
                var rook = GetPieceAt(board, baseRow, 0);
                if (rook != null)
                {
                    rook.Col = 3;
                }
                var rights = kingColor == "White" ? castlingRights.White : castlingRights.Black;
                rights.Short = false;
                rights.Long = false;
            }
        }

        // Simulate move and check if king would be in check
        public bool WouldKingBeInCheckAfterMove(
            List<Figure> board, Figure movingPiece, int fromRow, int fromCol, int toRow, int toCol)
        {
            // Clone board for simulation
            var simulatedBoard = board.Select(f =>
                new Figure
                {
                    Id = f.Id,
                    Type = f.Type,
                    Row = f.Row,
                    Col = f.Col,
                    Color = f.Color,
                    FigureImage = f.FigureImage,
                    CurrentPosition = f.CurrentPosition,
                    MoveHistory = new List<string>(f.MoveHistory)
                }).ToList();

            var moving = simulatedBoard.FirstOrDefault(f => f.Row == fromRow && f.Col == fromCol && f.Type == movingPiece.Type && f.Color == movingPiece.Color);
            if (moving == null) return false;
            // Remove any piece at toRow, toCol
            simulatedBoard.RemoveAll(f => f.Row == toRow && f.Col == toCol);
            // Move piece
            moving.Row = toRow;
            moving.Col = toCol;

            var king = FindKing(simulatedBoard, movingPiece.Color);
            if (king == null) return false;
            return IsSquareAttacked(simulatedBoard, king.Row, king.Col, movingPiece.Color);
        }

        // Main move handler (returns true if move is legal and updates board/rights/enpassant)
        public bool TryMove(
            List<Figure> board,
            int fromRow,
            int fromCol,
            int toRow,
            int toCol,
            string currentTurn,
            CastlingRights castlingRights,
            ref EnPassantTarget? enPassantTarget,
            out string? error)
        {
            error = null;
            var movingPiece = GetPieceAt(board, fromRow, fromCol);
            if (movingPiece == null) { error = "No piece at source."; return false; }
            var myColor = movingPiece.Color;
            var targetPiece = GetPieceAt(board, toRow, toCol);

            if (targetPiece != null && targetPiece.Color == myColor)
            {
                error = "Cannot capture own piece.";
                return false;
            }

            // Piece-specific legality
            if (movingPiece.Type.ToString() == "Pawn")
            {
                int direction = (myColor == "White") ? -1 : 1;
                int startRow = (myColor == "White") ? 6 : 1;
                int rowDiff = toRow - fromRow;
                int colDiff = toCol - fromCol;
                bool isForwardOne = (colDiff == 0 && rowDiff == direction && targetPiece == null);
                bool isForwardTwo = (colDiff == 0 && fromRow == startRow && rowDiff == 2 * direction &&
                    targetPiece == null && GetPieceAt(board, fromRow + direction, fromCol) == null);
                bool isCaptureDiagonal = (Math.Abs(colDiff) == 1 && rowDiff == direction &&
                    ((targetPiece != null && targetPiece.Color != myColor) ||
                        (enPassantTarget != null && enPassantTarget.Row == toRow && enPassantTarget.Col == toCol)));
                if (!(isForwardOne || isForwardTwo || isCaptureDiagonal))
                {
                    error = "Illegal pawn move.";
                    return false;
                }
            }
            if (movingPiece.Type.ToString() == "Bishop")
            {
                string fromSquareColor = IsWhiteSquare(fromRow, fromCol) ? "White" : "Black";
                string toSquareColor = IsWhiteSquare(toRow, toCol) ? "White" : "Black";
                if (!IsBishopMoveLegal(board, fromRow, fromCol, toRow, toCol, fromSquareColor, toSquareColor))
                {
                    error = "Illegal bishop move.";
                    return false;
                }
            }
            if (movingPiece.Type.ToString() == "Rook")
            {
                if (!IsRookMoveLegal(board, fromRow, fromCol, toRow, toCol))
                {
                    error = "Illegal rook move.";
                    return false;
                }
            }
            if (movingPiece.Type.ToString() == "Queen")
            {
                if (!IsQueenMoveLegal(board, fromRow, fromCol, toRow, toCol))
                {
                    error = "Illegal queen move.";
                    return false;
                }
            }
            if (movingPiece.Type.ToString() == "Knight")
            {
                if (!IsKnightMoveLegal(board, fromRow, fromCol, toRow, toCol, myColor))
                {
                    error = "Illegal knight move.";
                    return false;
                }
            }
            if (movingPiece.Type.ToString() == "King")
            {
                // Castling
                if (Math.Abs(toCol - fromCol) == 2 && fromRow == toRow)
                {
                    if (!IsCastlingMoveLegal(board, fromRow, fromCol, toRow, toCol, myColor, castlingRights))
                    {
                        error = "Illegal castling move.";
                        return false;
                    }
                    DoCastlingMove(board, fromRow, fromCol, toCol, myColor, castlingRights);
                }
                else if (!IsKingMoveLegal(board, fromRow, fromCol, toRow, toCol, myColor, castlingRights))
                {
                    error = "Illegal king move.";
                    return false;
                }
                var rights = myColor == "White" ? castlingRights.White : castlingRights.Black;
                rights.Short = false;
                rights.Long = false;
            }
            if (movingPiece.Type.ToString() == "Rook")
            {
                var rights = myColor == "White" ? castlingRights.White : castlingRights.Black;
                if (myColor == "White" && fromRow == 7 && fromCol == 0) rights.Long = false;
                if (myColor == "White" && fromRow == 7 && fromCol == 7) rights.Short = false;
                if (myColor == "Black" && fromRow == 0 && fromCol == 0) rights.Long = false;
                if (myColor == "Black" && fromRow == 0 && fromCol == 7) rights.Short = false;
            }

            // Simulate move to check for king in check before/after
            var kingPosBefore = FindKing(board, myColor);
            bool wasInCheck = IsSquareAttacked(board, kingPosBefore.Row, kingPosBefore.Col, myColor);
            bool isStillInCheck = WouldKingBeInCheckAfterMove(board, movingPiece, fromRow, fromCol, toRow, toCol);

            if (wasInCheck && isStillInCheck)
            {
                error = "Move does not resolve check.";
                return false;
            }
            if (!wasInCheck && isStillInCheck)
            {
                error = "Move puts king in check.";
                return false;
            }

            // EN PASSANT CAPTURE (remove the captured pawn)
            if (movingPiece.Type.ToString() == "Pawn"
                && enPassantTarget != null
                && toRow == enPassantTarget.Row
                && toCol == enPassantTarget.Col)
            {
                int capturedRow = currentTurn == "White" ? toRow + 1 : toRow - 1;
                var capturedPawn = GetPieceAt(board, capturedRow, toCol);
                if (capturedPawn != null && capturedPawn.Type.ToString() == "Pawn" && capturedPawn.Color != myColor)
                {
                    board.Remove(capturedPawn);
                }
            }

            // Remove any enemy piece (capture)
            if (targetPiece != null && targetPiece.Color != myColor)
            {
                board.Remove(targetPiece);
            }

            // Move the piece
            movingPiece.Row = toRow;
            movingPiece.Col = toCol;

            // Set en passant target square if pawn just moved 2 squares
            enPassantTarget = null;
            if (movingPiece.Type.ToString() == "Pawn" && Math.Abs(toRow - fromRow) == 2)
            {
                enPassantTarget = new EnPassantTarget
                {
                    Row = (fromRow + toRow) / 2,
                    Col = fromCol
                };
            }

            return true;
        }
    }
}
