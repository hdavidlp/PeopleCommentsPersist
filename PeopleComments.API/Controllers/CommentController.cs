using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using PeopleComments.Dll.Entities;
using PeopleComments.Dll.Models.Comment;
using PeopleComments.Dll.Services;

namespace PeopleComments.API.Controllers
{
    [Route("api/account/{accountId}/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly IAccountCommentInfoRepository _accountCommentInfoRepository;
        private readonly ICommentInfoRepository _commentInfoRepository;
        private readonly IMapper _mapper;

        public CommentController(
            IAccountCommentInfoRepository accountCommentInfoRepository, 
            ICommentInfoRepository commentInfoRepository,
            IMapper mapper)
        {
            _accountCommentInfoRepository = accountCommentInfoRepository ??
                throw new ArgumentNullException(nameof(accountCommentInfoRepository));
            _commentInfoRepository = commentInfoRepository??
                throw new ArgumentNullException(nameof(commentInfoRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetCommentsOfAccount(int accountId)
        {
            if (!await _accountCommentInfoRepository.AccountExistsAsync(accountId))
            {
                return NotFound();
            }

            var commentsForAccount = await _commentInfoRepository
                .GetCommentsForAccountAsync(accountId);

            return Ok(_mapper.Map<IEnumerable<CommentDto>>(commentsForAccount));
        }

        [HttpGet("{commentId}", Name="GetComment")]
        public async Task<ActionResult<CommentDto>> GetComment(
            int accountId, int commentId)
        {
            if (!await _accountCommentInfoRepository.AccountExistsAsync(accountId))
            {
                return NotFound();
            }

            var comment = await _commentInfoRepository
                .GetCommentForAccount(accountId, commentId);
            if (comment == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<CommentDto>(comment));
        }

        [HttpPost]
        public async Task<ActionResult<CommentDto>> CreateComment(
            int accountId,
            CommentForCreationDto comment) 
        {
            var newComment = _mapper.Map<Comment>(comment);

            if(!await _commentInfoRepository.AddCommentForAccountAsync(
                accountId, newComment))
            {
                return NotFound();
            }

            var (createdCommentToReturn, infoAccountIdAndCommentId) =
                _commentInfoRepository.convertoComment(accountId, newComment);

            return CreatedAtRoute("GetComment",
                infoAccountIdAndCommentId,
                createdCommentToReturn);
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
                .GetCommentForAccount(accountId, commentId);
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
                .GetCommentForAccount(accountId, commentId);
            
            if (commentEntity == null)
            {
                return NotFound();
            }

            _commentInfoRepository.DeleteCommentForAccount(commentEntity);
            await _commentInfoRepository.SaveChangesAsync();

            return NoContent();
        }
    }
}
