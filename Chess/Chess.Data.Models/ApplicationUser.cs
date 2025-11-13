namespace Chess.Data.Models;

using Microsoft.AspNetCore.Identity;

public class ApplicationUser : IdentityUser
{
    public ApplicationUser()
    {
        Boards = new HashSet<UserBoard>();
    }

    public IEnumerable<UserBoard> Boards { get; set; }
}
