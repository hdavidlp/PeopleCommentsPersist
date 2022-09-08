using Microsoft.EntityFrameworkCore;
using PeopleComments.Data.DbContexts;
using PeopleComments.Data.Entities;
using PeopleComments.Data.Models;

namespace PeopleComments.Data.Services
{
    public class AccountCommentInfoRepository : IAccountCommentInfoRepository
    {
        private readonly AccountCommentsContext _context;

        public AccountCommentInfoRepository(AccountCommentsContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> AccountExistsAsync(int accountId)
        {
            return await _context.Accounts.AnyAsync(c => c.Id == accountId);
        }


        public async Task<IEnumerable<Account>> GetAccountsAsync()
        {
            var collection = _context.Accounts as IQueryable<Account>;
            var collectionToReturn = await collection.OrderBy(c => c.Name)
                .ToListAsync();
            return collectionToReturn;
        }

        public async Task<(IEnumerable<Account>, PaginationMetaData)> GetAccountsAsync(
            string? name, string? searchQuery, int pageNumber, int pageSize)
        {
            var collection = _context.Accounts as IQueryable<Account>;

            if (!string.IsNullOrWhiteSpace(name))
            {
                name = name.Trim();
                collection = collection.Where(c => c.Name == name);
            }

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                collection = collection.Where(a => a.Name.Contains(searchQuery)
                    || (a.Email != null && a.Email.Contains(searchQuery)));
            }

            var totalItemCount = await collection.CountAsync();
            var paginationMetaData = new PaginationMetaData(
                totalItemCount, pageSize, pageNumber);

            var collectionToReturn = await collection.OrderBy(c => c.Name)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();

            return (collectionToReturn, paginationMetaData);

        }


        public async Task<Account?> GetAccountAsync(int id)
        {
            return await _context.Accounts
                .Where(a => a.Id == id).FirstOrDefaultAsync();
        }


        public Task AddAccount(Account account)
        {
            _context.Accounts.Add(account);
            return Task.CompletedTask;
        }


        public void DeleteAccount(Account account)
        {
            _context.Accounts.Remove(account);  
        }

        public async Task<IEnumerable<Comment>> GetCommentsForAccountAsync(int accountId)
        {
            return await _context.Comments
                .Where(p => p.AccountId == accountId).ToListAsync();
        }

        public async Task<Comment?> GetCommentForAccount(
            int accountId,
            int commentId)
        {
            return await _context.Comments
                .Where(c => c.AccountId == accountId && c.Id == commentId)
                .FirstAsync();
        }

        public async Task AddCommentForAccountAsync(int accountId, Comment comment)
        {
            var account = await GetAccountAsync(accountId);
            if (account != null)
            {
                account.Comments.Add(comment);
            }
        }

        public void DeleteCommentForAccount(Comment comment)
        {
            _context.Comments.Remove(comment);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }

        
    }
}
