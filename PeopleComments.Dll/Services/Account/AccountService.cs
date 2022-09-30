using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using PeopleComments.Dll.Entities;
using PeopleComments.Dll.Models.Account;
using PeopleComments.Dll.Repositories.Account;
using PeopleComments.Dll.Repositories.Comment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PeopleComments.Dll.Services.Account
{
    public class AccountService:IAccountService
    {

        private const int maxPageSize = 10;
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;


        public AccountService(
            IAccountRepository accountRepository,
            IMapper mapper
            )
        {
            _accountRepository = accountRepository ??
                throw new ArgumentNullException(nameof(accountRepository));
            _mapper = mapper ?? 
                throw new ArgumentNullException(nameof(mapper));
        }

        public Task<bool> AccountExistsAsync(int accountId)
        {
            return _accountRepository.AccountExistAsync(accountId);
        } 

        public async Task<Entities.Account?> GetAccountAsync (int accountId)
        {
            return await _accountRepository.GetAccountAsync(accountId); 
        }

        public async Task<IEnumerable<Entities.Account>> GetAccountsAsync()
        {
            return await _accountRepository.GetAccountsAsync();
        }


        public async Task<(IEnumerable<Entities.Account>, PaginationMetaData)> GetAccountsAsync(
            string? name, string? searchQuery, int pageNumber, int pageSize)
        {


            if (pageSize > maxPageSize) pageSize = maxPageSize;

            var (accountEntities, paginationMetaData) = await _accountRepository
                .GetAccountsAsync(name, searchQuery, pageNumber, pageSize);

            return (accountEntities, paginationMetaData);
            
        }



        public async Task<IEnumerable<AccountWithoutCommentsDto>> GetAccountsWithoutCommentsAsync()
        {
            var accountsSelected =  await _accountRepository.GetAccountsAsync();
            return _mapper.Map<IEnumerable<AccountWithoutCommentsDto>>(accountsSelected);
        }

        public async Task<bool> AddAccount(Entities.Account newAccount)
        {
            bool result = await _accountRepository.AddAccount(newAccount);

            return result;
        }
        public async Task<bool> UpdateAccount(int id, AccountForUpdateDto account)
        {
            var selectedAccount = await GetAccountAsync(id);

            _mapper.Map(account, selectedAccount);
            await _accountRepository.SaveChangesAsync();

            return true;

        }
        public async Task<bool> DeleteAccount(Entities.Account account)
        {
            _accountRepository.DeleteAccount(account);
            _accountRepository.SaveChangesAsync();
            return true;
        }


        public async Task<bool> SaveChangesAsync()
        {
            return await _accountRepository.SaveChangesAsync();
        }


    }
}
