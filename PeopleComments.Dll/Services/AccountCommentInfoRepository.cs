﻿using Microsoft.EntityFrameworkCore;
using PeopleComments.Dll.DbContexts;
using PeopleComments.Dll.Entities;
using PeopleComments.Dll.Services;
using PeopleComments.Dll.Models;
using PeopleComments.Dll.Models.Comment;
using AutoMapper;


namespace PeopleComments.Dll.Services
{
    public class AccountCommentInfoRepository : IAccountCommentInfoRepository
    {
        private readonly AccountCommentsContext _context;
        private readonly IMapper _mapper;

        public AccountCommentInfoRepository(AccountCommentsContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<bool> AccountExistsAsync(int accountId)
        {
            return await _context.Accounts.AnyAsync(c => c.Id == accountId);
        }


        public async Task<IEnumerable<Account>> GetAccountsAsync()
        {
            var collection = _context.Accounts as IQueryable<Account>;
            var collectionToReturn = await collection.OrderBy(c => c.Name)
                .ToListAsync();
            return collectionToReturn;
        }

        public async Task<(IEnumerable<Account>, PaginationMetaData)> GetAccountsAsync(
            string? name, string? searchQuery, int pageNumber, int pageSize)
        {
            var collection = _context.Accounts as IQueryable<Account>;

            if (!string.IsNullOrWhiteSpace(name))
            {
                name = name.Trim();
                collection = collection.Where(c => c.Name == name);
            }

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                collection = collection.Where(a => a.Name.Contains(searchQuery)
                    || (a.Email != null && a.Email.Contains(searchQuery)));
            }

            var totalItemCount = await collection.CountAsync();
            var paginationMetaData = new PaginationMetaData(
                totalItemCount, pageSize, pageNumber);

            var collectionToReturn = await collection.OrderBy(c => c.Name)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();

            return (collectionToReturn, paginationMetaData);

        }


        public async Task<Account?> GetAccountAsync(int id)
        {
            return await _context.Accounts
                .Where(a => a.Id == id).FirstOrDefaultAsync();
        }


        public Task AddAccount(Account account)
        {
            _context.Accounts.Add(account);
            return Task.CompletedTask;
        }


        public void DeleteAccount(Account account)
        {
            _context.Accounts.Remove(account);  
        }

        //public async Task<IEnumerable<Comment>> GetCommentsForAccountAsync(int accountId)
        //{
        //    return await _context.Comments
        //        .Where(p => p.AccountId == accountId).ToListAsync();
        //}

        //public async Task<Comment?> GetCommentForAccount(
        //    int accountId,
        //    int commentId)
        //{
        //    return await _context.Comments
        //        .Where(c => c.AccountId == accountId && c.Id == commentId)
        //        .FirstAsync();
        //}

        //public (CommentDto, Object) convertoComment(int accountId, Comment newComment)
        //{
        //    var createdCommentToReturn =
        //        _mapper.Map<CommentDto>(newComment);

        //    var x = new { accountId = accountId, commentId = createdCommentToReturn.Id };

        //    return (createdCommentToReturn, x);
        //}

        //public async Task<bool> AddCommentForAccountAsync(int accountId, Comment comment)
        //{

        //    if (!await AccountExistsAsync(accountId))
        //    {
        //        return false;
        //    }

        //    var account = await GetAccountAsync(accountId);
        //    if (account != null)
        //    {
        //        account.Comments.Add(comment);
        //    }

        //    await SaveChangesAsync();

        //    return true;
        //}



        //public void DeleteCommentForAccount(Comment comment)
        //{
        //    _context.Comments.Remove(comment);
        //}


        // ************************************************************

        //public async Task<IEnumerable<Account>> GetCommentsAsync()
        //{
        //    return await _context.Accounts.Include(c => c.Comments).ToListAsync();
        //    //return await _context.Comments.Include(c=> c) .ToListAsync();
        //}


        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }

    }
}
