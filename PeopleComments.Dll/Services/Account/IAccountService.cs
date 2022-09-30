using PeopleComments.Dll.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleComments.Dll.Services.Account
{
    public interface IAccountService
    {
        Task<bool> AccountExistsAsync(int accountId);
        Task<Entities.Account?> GetAccountAsync(int accountId);
        Task<IEnumerable<Entities.Account>> GetAccountsAsync();
        Task<(IEnumerable<Entities.Account>, PaginationMetaData)> GetAccountsAsync(
            string? name, string? searchQuery, int pageNumber, int pageSize);
        Task<IEnumerable<AccountWithoutCommentsDto>> GetAccountsWithoutCommentsAsync();
        Task<bool> AddAccount(Entities.Account newAccount);
        Task<bool> UpdateAccount(int id, AccountForUpdateDto account);
        Task<bool> DeleteAccount(Entities.Account account);
        Task<bool> SaveChangesAsync();

    }
}
