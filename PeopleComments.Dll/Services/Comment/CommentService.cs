﻿using AutoMapper;
using PeopleComments.Dll.Entities;
using PeopleComments.Dll.Models.Comment;
using PeopleComments.Dll.Repositories.Account;
using PeopleComments.Dll.Repositories.Comment;
using PeopleComments.Dll.Services.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleComments.Dll.Services.Comment
{
    public class CommentService : ICommentService
    {
        private readonly ICommentInfoRepository _commentInfoRepository;
        private readonly IAccountService _accountService;   
        private readonly IMapper _mapper;

        public CommentService(
            ICommentInfoRepository commentInfoRepository, 
            IAccountService accountService,
            IMapper mapper)
        {
            _commentInfoRepository  = commentInfoRepository ?? 
                throw new ArgumentNullException(nameof(commentInfoRepository)); 
            _mapper = mapper ?? 
                throw new ArgumentNullException(nameof(mapper));

            _accountService = accountService ?? 
                throw new ArgumentNullException(nameof(accountService));
        }

        public async Task<bool> CommentExistsAsync(int commentId)
        {
            return await _commentInfoRepository.CommentExistsAsync(commentId);
        }


        public async Task<IEnumerable<CommentDto>> GetCommentsForAccountAsync(int accountId)
        {

            if (!await _accountService.AccountExistsAsync(accountId))
            {
                return  null; // return NotFound();
            }

            var commentsForAccount = await _commentInfoRepository.GetCommentsForAccountAsync(accountId);

            return _mapper.Map<IEnumerable<CommentDto>>(commentsForAccount);
        }


        public async Task<CommentDto> GetCommentForAccountAsync(int accountId, int commentId)
        {
            if (!await _accountService.AccountExistsAsync(accountId))
                return null;

            if (!await CommentExistsAsync(commentId))
                return null;

            var commentForAccount = await _commentInfoRepository.GetCommentForAccountAsync(accountId, commentId);

            return _mapper.Map<CommentDto>(commentForAccount);
        }

        public async Task<bool> AddCommentForAccountAsync(int accountId, CommentForCreationDto commentDto)
        {
            
            if (!await _accountService.AccountExistsAsync(accountId))
                return false;

            if (commentDto == null)
                return false;

            bool addCommentResult = await _commentInfoRepository.AddCommentForAccountAsync(
                                accountId, 
                                _mapper.Map<Entities.Comment>(commentDto));

            return addCommentResult;
        }

        public (CommentDto, object) convertoComment(int accountId, Entities.Comment newComment)
        {
            var createdCommentToReturn =
                _mapper.Map<CommentDto>(newComment);

            var x = new { accountId, commentId = createdCommentToReturn.Id };

            return (createdCommentToReturn, x);
        }




    }
}