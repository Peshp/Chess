namespace Chess.Data.Models
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Identity;

    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            this.Boards = new HashSet<Board>();
        }

        public ICollection<Board> Boards { get; set; }
    }
}
