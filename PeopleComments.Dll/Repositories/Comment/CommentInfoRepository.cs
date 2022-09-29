using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PeopleComments.Dll.DbContexts;
using PeopleComments.Dll.Entities;
using PeopleComments.Dll.Models.Comment;
using PeopleComments.Dll.Repositories.Account;
using PeopleComments.Dll.Services.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PeopleComments.Dll.Repositories.Comment
{
    public class CommentInfoRepository : ICommentInfoRepository
    {
        private readonly AccountCommentsContext _context;
        private readonly IMapper _mapper;
        private readonly IAccountCommentInfoRepository _accountCommentInfoRepository;

        private readonly IAccountService _accountService;

        public CommentInfoRepository(
            AccountCommentsContext context,
            IMapper mapper,
            IAccountCommentInfoRepository accountCommentInfoRepository,
            
            IAccountService accountService
            
            )
        {
            _context = context ??
                throw new ArgumentNullException(nameof(context));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            _accountCommentInfoRepository = accountCommentInfoRepository ??
                throw new ArgumentNullException(nameof(accountCommentInfoRepository));

            _accountService = accountService ??
                throw new ArgumentNullException(nameof(accountService));

        }

        public async Task<bool> CommentExistsAsync(int commentId)
        {
            return await _context.Comments.AnyAsync(c => c.Id == commentId);
        }

        public async Task<IEnumerable<Entities.Comment>> GetCommentsForAccountAsync(int accountId)
        {
            return await _context.Comments
                .Where(p => p.AccountId == accountId).ToListAsync();
        }

        public async Task<Entities.Comment?> GetCommentForAccountAsync(int accountId, int commentId)
        {
            return await _context.Comments
                .Where(p => p.AccountId == accountId && p.Id == commentId)
                .FirstAsync();
        }



        public async Task<bool> AddCommentForAccountAsync(int accountId, Entities.Comment comment)
        {
            if (!await _accountService.AccountExistsAsync(accountId))
                return false;

            var account = await _accountService.GetAccountAsync(accountId);

            if (account!= null)
            {
                account.Comments.Add(comment);
            }

            await SaveChangesAsync();
            return true;
        }


        public async void DeleteCommentForAccountAsync(Entities.Comment comment)
        {
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Entities.Account>> GetCommentsAsync()
        {
            return await _context.Accounts.Include(c => c.Comments).ToListAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }

    }
}
