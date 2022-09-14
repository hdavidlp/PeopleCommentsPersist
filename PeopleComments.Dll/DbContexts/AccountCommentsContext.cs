using Microsoft.EntityFrameworkCore;
using PeopleComments.Dll.Entities;

namespace PeopleComments.Dll.DbContexts
{
    public class AccountCommentsContext: DbContext
    {
        public DbSet<Account> Accounts { get; set; } = null!;
        public DbSet<Comment> Comments { get; set; } = null!;

        public AccountCommentsContext(DbContextOptions<AccountCommentsContext> options)
            : base(options)
        {

        }
    }
}
