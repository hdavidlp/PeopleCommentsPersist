using PeopleComments.Dll.Models.Comment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleComments.Dll.Services.Comment
{
    public  interface ICommentService
    {

        //IEnumerable<CommentDto> commentsListFromEntityToDto(IEnumerable<Entities.Comment> commentsForAccount);
        
        Task<IEnumerable<CommentDto>> GetCommentsForAccountAsync(int accountId);

        Task<CommentDto?> GetCommentForAccountAsync(int accountId, int commentId);
        Task<bool> AddCommentForAccountAsync(int accountId, CommentForCreationDto commentDto);


        (CommentDto, object) convertoComment(int accountId, Entities.Comment newComment);



    }
}
