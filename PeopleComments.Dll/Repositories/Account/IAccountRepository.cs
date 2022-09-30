using PeopleComments.Dll.Services;

namespace PeopleComments.Dll.Repositories.Account
{
    public interface IAccountRepository
    {

        Task<bool> AccountExistAsync(int accountId);
        Task<IEnumerable<Entities.Account>> GetAccountsAsync();
        Task<(IEnumerable<Entities.Account>, PaginationMetaData)> GetAccountsAsync(
            string? name, string? searchQuery, int pageNumber, int pageSize);
        Task<Entities.Account?> GetAccountAsync(int accountId);
        Task<bool> AddAccount(Entities.Account account);
        void DeleteAccount(Entities.Account account);
        Task<bool> SaveChangesAsync();
    }
}