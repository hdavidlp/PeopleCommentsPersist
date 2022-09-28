using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using PeopleComments.Dll.Entities;
using PeopleComments.Dll.Models.Comment;
using PeopleComments.Dll.Repositories.Account;
using PeopleComments.Dll.Repositories.Comment;
using PeopleComments.Dll.Services.Account;
using PeopleComments.Dll.Services.Comment;

namespace PeopleComments.API.Controllers
{
    [Route("api/account/{accountId}/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly IAccountCommentInfoRepository _accountCommentInfoRepository;
        private readonly ICommentInfoRepository _commentInfoRepository;
        private readonly IMapper _mapper;

        private readonly ICommentService _commentService;
        private readonly IAccountService _accountService;

        public CommentController(
            IAccountCommentInfoRepository accountCommentInfoRepository, 
            ICommentInfoRepository commentInfoRepository,
            IMapper mapper,


            ICommentService commentService,
            IAccountService accountService
            )
        {
            _accountCommentInfoRepository = accountCommentInfoRepository ??
                throw new ArgumentNullException(nameof(accountCommentInfoRepository));
            _commentInfoRepository = commentInfoRepository??
                throw new ArgumentNullException(nameof(commentInfoRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));

            _commentService = commentService ?? throw new ArgumentNullException(nameof(commentService));
            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetCommentsOfAccount(int accountId)
        {
            var commentsOfAccount = 
                await _commentService.GetCommentsForAccountAsync(accountId);

            if (!(commentsOfAccount is null))
                return Ok(commentsOfAccount);
            else
                return NotFound();
        }


        [HttpGet("{commentId}", Name="GetComment")]
        public async Task<ActionResult<CommentDto>> GetComment(
            int accountId, int commentId)
        {
            var commentForAccount = 
                await _commentService.GetCommentForAccountAsync(accountId, commentId);

            if (!(commentForAccount is null))
                return Ok(commentForAccount);
            else
                return NotFound();

        }

        [HttpPost]
        public async Task<ActionResult<CommentDto>> CreateComment(
            int accountId,
            CommentForCreationDto comment) 
        {

            var newComment = comment;
            bool creationResult = await _commentService.AddCommentForAccountAsync(accountId, newComment);

            if (creationResult)
            {
                var (createdCommentToReturn, infoAccountIdAndCommentId) =
                    _commentService.convertoComment(accountId, _mapper.Map<Comment>(newComment));

                return CreatedAtRoute("GetComment",
                    infoAccountIdAndCommentId,
                    createdCommentToReturn);
                
            }
            else
                return NotFound();

            //var newComment = _mapper.Map<Comment>(comment);

            //if(!await _commentInfoRepository.AddCommentForAccountAsync(
            //    accountId, newComment))
            //{
            //    return NotFound();
            //}

            //var (createdCommentToReturn, infoAccountIdAndCommentId) =
            //    _commentInfoRepository.convertoComment(accountId, newComment);

            //return CreatedAtRoute("GetComment",
            //    infoAccountIdAndCommentId,
            //    createdCommentToReturn);
        }

        [HttpPut("{commentid}")]
        public async Task<ActionResult> UpdateComment(
            int accountId,
            int commentId,
            CommentForUpdateDto comment)
        {
            if (!await _accountCommentInfoRepository.AccountExistsAsync(accountId))
            {
                return NotFound();
            }

            var commentEntity = await _commentInfoRepository
                .GetCommentForAccountAsync(accountId, commentId);
            if (commentEntity == null)
            {
                return NotFound();
            }

            _mapper.Map(comment, commentEntity);

            await _accountCommentInfoRepository.SaveChangesAsync();
            return NoContent();
        }

        


        [HttpDelete("{commentId}")]
        public async Task<ActionResult> DeleteCommentOfAccount(
            int accountId,
            int commentId)
        {
            if (!await _accountCommentInfoRepository.AccountExistsAsync(accountId))
            {
                return NotFound();
            }

            var commentEntity = await _commentInfoRepository
                .GetCommentForAccountAsync(accountId, commentId);
            
            if (commentEntity == null)
            {
                return NotFound();
            }

            _commentInfoRepository.DeleteCommentForAccountAsync(commentEntity);
            await _commentInfoRepository.SaveChangesAsync();

            return NoContent();
        }
    }
}
