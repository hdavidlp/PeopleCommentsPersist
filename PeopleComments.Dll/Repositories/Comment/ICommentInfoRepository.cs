using PeopleComments.Dll.Entities;
using PeopleComments.Dll.Models.Comment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleComments.Dll.Repositories.Comment
{
    public interface ICommentInfoRepository
    {
        Task<bool> CommentExistsAsync(int commentId);
        Task<IEnumerable<Entities.Comment>> GetCommentsForAccountAsync(int accountId);

        Task<Entities.Comment?> GetCommentForAccountAsync(int accountId, int commentId);

        Task<IEnumerable<Entities.Account>> GetCommentsAsync();

        Task<bool> AddCommentForAccountAsync(int accountId, Entities.Comment comment);

        void DeleteCommentForAccountAsync(Entities.Comment comment);

        Task<bool> SaveChangesAsync();

    }
}
