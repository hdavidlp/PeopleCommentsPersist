using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PeopleComments.Dll.DbContexts;
using PeopleComments.Dll.Entities;
using PeopleComments.Dll.Models.Comment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PeopleComments.Dll.Services
{
    public class CommentInfoRepository: ICommentInfoRepository
    {
        private readonly AccountCommentsContext _context;
        private readonly IMapper _mapper;
        private readonly IAccountCommentInfoRepository _accountCommentInfoRepository;

        public CommentInfoRepository(
            AccountCommentsContext context, 
            IMapper mapper, 
            IAccountCommentInfoRepository accountCommentInfoRepository)
        {
            _context = context ?? 
                throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? 
                throw new ArgumentNullException(nameof(mapper));
            _accountCommentInfoRepository = accountCommentInfoRepository ?? 
                throw new ArgumentNullException(nameof(accountCommentInfoRepository));
        }

        public async Task<IEnumerable<Comment>> GetCommentsForAccountAsync(int accountId)
        {
            return await _context.Comments
                .Where(p => p.AccountId == accountId).ToListAsync();
        }

        public async Task < Comment?> GetCommentForAccount (int accountId, int commentId)
        {
            return await _context.Comments
                .Where(p => p.AccountId == accountId && p.Id == commentId)
                .FirstAsync();
        }

        public async Task<IEnumerable<Account>> GetCommentsAsync()
        {
            return await _context.Accounts.Include(c => c.Comments).ToListAsync();
        }

        public (CommentDto, Object) convertoComment(int accountId, Comment newComment)
        {
            var createdCommentToReturn =
                _mapper.Map<CommentDto>(newComment);

            var x = new { accountId = accountId, commentId = createdCommentToReturn.Id };

            return (createdCommentToReturn, x);
        }

        public async Task<bool> AddCommentForAccountAsync(int accountId, Comment comment)
        {

            if (!await _accountCommentInfoRepository.AccountExistsAsync(accountId))
            {
                return false;
            }

            var account = await _accountCommentInfoRepository.GetAccountAsync(accountId);
            if (account != null)
            {
                account.Comments.Add(comment);
            }

            await SaveChangesAsync();

            return true;
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
