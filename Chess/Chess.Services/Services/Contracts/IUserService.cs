namespace Chess.Services.Services.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Chess.Web.ViewModels.User;

    public interface IUserService
    {
        Task<IEnumerable<UserBoardsViewModel>> GetHistory(string userId);

        Task<UserBoardsViewModel> BoardDetails(int Id);
    }
}
