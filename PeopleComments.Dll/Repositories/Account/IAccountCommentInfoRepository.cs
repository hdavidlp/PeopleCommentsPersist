using PeopleComments.Dll.Entities;
using PeopleComments.Dll.Models;
using PeopleComments.Dll.Models.Comment;
using PeopleComments.Dll.Models.LogList;
using PeopleComments.Dll.Services;

namespace PeopleComments.Dll.Repositories.Account
{
    public interface IAccountCommentInfoRepository
    {

        // Accounts Actions
        Task<bool> AccountExistsAsync(int accountId);
        Task<IEnumerable<Entities.Account>> GetAccountsAsync();
        Task<(IEnumerable<Entities.Account>, PaginationMetaData)> GetAccountsAsync(string? name, string? searchQuery, int pageNumber, int pageSize);
        Task<Entities.Account?> GetAccountAsync(int id);
        Task AddAccount(Entities.Account account);
        void DeleteAccount(Entities.Account account);


        //Comments actions
        //Task<IEnumerable<Comment>> GetCommentsForAccountAsync(int accountId);
        //Task<Comment?> GetCommentForAccount(int accountId, int commentId);
        //(CommentDto, Object) convertoComment(int accountId, Comment newComment);
        //Task<bool> AddCommentForAccountAsync(int accountId, Comment comment);

        //void DeleteCommentForAccount(Comment comment);
        //Task<IEnumerable<Account>> GetCommentsAsync();

        Task<bool> SaveChangesAsync();


    }
}
