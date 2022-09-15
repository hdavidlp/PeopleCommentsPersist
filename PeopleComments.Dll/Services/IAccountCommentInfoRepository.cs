using PeopleComments.Dll.Entities;
using PeopleComments.Dll.Models;
using PeopleComments.Dll.Models.LogList;

namespace PeopleComments.Dll.Services
{
    public interface IAccountCommentInfoRepository
    {

        // Accounts Actions
        Task<bool> AccountExistsAsync(int accountId);
        Task<IEnumerable<Account>> GetAccountsAsync();
        Task<(IEnumerable<Account>, PaginationMetaData)> GetAccountsAsync(string? name, string? searchQuery, int pageNumber, int pageSize);
        Task<Account?> GetAccountAsync(int id);
        Task AddAccount(Account account);
        void DeleteAccount(Account account);


        // Comments actions
        Task<IEnumerable<Comment>> GetCommentsForAccountAsync(int accountId);
        Task<Comment?> GetCommentForAccount(int accountId, int commentId);

        Task AddCommentForAccountAsync(int accountId, Comment comment);

        void DeleteCommentForAccount(Comment comment);

        Task<bool> SaveChangesAsync();


        Task<IEnumerable<Account>> GetCommentsAsync();


    }
}
