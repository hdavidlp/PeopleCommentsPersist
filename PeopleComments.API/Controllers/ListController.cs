using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PeopleComments.Dll.Entities;
using PeopleComments.Dll.Models.Comment;
using PeopleComments.Dll.Services;
using PeopleComments.Dll.Models.Account;

namespace PeopleComments.API.Controllers
{
    [Route("api/list")]
    [ApiController]
    public class ListController : ControllerBase
    {
        private readonly IAccountCommentInfoRepository _accountCommentInfoRepository;
        private readonly IMapper _mapper;

        public ListController(IAccountCommentInfoRepository accountCommentInfoRepository, IMapper mapper)
        {
            _accountCommentInfoRepository = accountCommentInfoRepository ??
                throw new ArgumentNullException(nameof(accountCommentInfoRepository));
            _mapper = mapper ?? 
                throw new ArgumentNullException(nameof(mapper));
            
        }


        [HttpGet("comments")]
        public async Task<ActionResult<IEnumerable<AccountWithCommentListDto>>> GetAllComments()
        {
            // List Long Comments mix 2 Entities
            // Account.Name  Comment.CommentDetail Comment.Date
            //    Jhon              Hi               22/09/15

            var allComments = await _accountCommentInfoRepository.GetCommentsAsync();
            List<AccountWithCommentListDto> result = new List<AccountWithCommentListDto>();  

            foreach (var account in allComments)
            {
                foreach (var comment in account.Comments)
                {
                    var itemAccountComment = new AccountWithCommentListDto();

                    itemAccountComment.Id = comment.Id;
                    itemAccountComment.Name = account.Name;
                    itemAccountComment.Email = account.Email;
                    itemAccountComment.CommentDetail = comment.CommentDetail;
                    itemAccountComment.Date = comment.Date;
                    
                    result.Add(itemAccountComment);
                } 
            }
            return Ok(result.ToList());

        }

        [HttpGet("wc")]
        public ActionResult Veremos()
        {
            var exampleComment = new Comment { Id=1, CommentDetail="Example comment", Date=DateTime.Now, AccountId=1 };
            var account = new Account("Jhon");
            
            account.Id = 1;
            account.Comments = new List<Comment> { exampleComment };

            return Ok(account);
        }
    }
}
