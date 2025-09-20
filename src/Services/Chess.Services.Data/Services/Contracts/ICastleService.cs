using Chess.Web.ViewModels.Chess;
using System;
using System.Threading.Tasks;

namespace Chess.Services.Data.Services.Contracts
{
    public interface ICastleService
    {
        Task<bool> IsCastleLegal(BoardViewModel board, FigureViewModel king, double toX, double toY);

        void PerformCastleMove(FigureViewModel king, BoardViewModel board, double toX, double toY);

        bool IsCastleAttempt(FigureViewModel king, double toX, double toY);
    }
}
