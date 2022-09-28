using PeopleComments.Dll.Repositories.Account;
using PeopleComments.Dll.Repositories.Comment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleComments.Dll.Services.Account
{
    public class AccountService:IAccountService
    {
        private readonly IAccountCommentInfoRepository _accountCommentInfoRepository;
        public AccountService(IAccountCommentInfoRepository accountCommentInfoRepository )
        {
            _accountCommentInfoRepository = accountCommentInfoRepository ?? 
                throw new ArgumentNullException(nameof(accountCommentInfoRepository));
            
        }

        public Task<bool> AccountExistsAsync(int accountId)
        {
            return _accountCommentInfoRepository.AccountExistsAsync(accountId);
        }

        public async Task<Entities.Account?> GetAccountAsync (int accountId)
        {
            return await _accountCommentInfoRepository.GetAccountAsync(accountId); 
        }
    }
}
