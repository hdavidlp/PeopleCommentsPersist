using PeopleComments.Dll.Entities;
using PeopleComments.Dll.Models.Comment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleComments.Dll.Services
{
    public interface ICommentInfoRepository
    {
        Task<IEnumerable<Comment>> GetCommentsForAccountAsync(int accountId);

        Task<Comment?> GetCommentForAccount(int accountId, int commentId);

        Task<IEnumerable<Account>> GetCommentsAsync();

        (CommentDto, Object) convertoComment(int accountId, Comment newComment);

        Task<bool> AddCommentForAccountAsync(int accountId, Comment comment);

        void DeleteCommentForAccount(Comment comment);

        Task<bool> SaveChangesAsync();

    }
}
