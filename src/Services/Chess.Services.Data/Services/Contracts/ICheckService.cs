using Chess.Web.ViewModels.Chess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Services.Data.Services.Contracts
{
    public interface ICheckService
    {
        Task<bool> IsCheck(BoardViewModel board, string color);

        Task<bool> IsLegalMove(BoardViewModel board, FigureViewModel piece, double toX, double toY);

        Task<bool> HasLegalMoveToEscapeCheck(BoardViewModel board, string color);
    }
}
