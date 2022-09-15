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
        private readonly IMapper _mapper;

        public CommentController(IAccountCommentInfoRepository accountCommentInfoRepository, IMapper mapper)
        {
            _accountCommentInfoRepository = accountCommentInfoRepository ??
                throw new ArgumentNullException(nameof(accountCommentInfoRepository));
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

            var commentsForAccount = await _accountCommentInfoRepository
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

            var comment = await _accountCommentInfoRepository
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
            CommentForCreationDto comment) // This parameter is [FromBody]
        {
            if (!await _accountCommentInfoRepository.AccountExistsAsync(accountId))
            {
                return NotFound();
            }

            var newComment = _mapper.Map<Comment>(comment);

            await _accountCommentInfoRepository.AddCommentForAccountAsync(
                accountId, newComment);
            await _accountCommentInfoRepository.SaveChangesAsync();

            var createdCommentToReturn =
                _mapper.Map<CommentDto>(newComment);

            return CreatedAtRoute("GetComment",
                new
                {
                    accountId = accountId,
                    commentId = createdCommentToReturn.Id
                },
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

            var commentEntity = await _accountCommentInfoRepository
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

            var commentEntity = await _accountCommentInfoRepository
                .GetCommentForAccount(accountId, commentId);
            
            if (commentEntity == null)
            {
                return NotFound();
            }

            _accountCommentInfoRepository.DeleteCommentForAccount(commentEntity);
            await _accountCommentInfoRepository.SaveChangesAsync();

            return NoContent();
        }
    }
}
