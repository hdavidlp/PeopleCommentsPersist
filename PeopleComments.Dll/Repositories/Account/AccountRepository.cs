using PeopleComments.Dll.DbContexts;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PeopleComments.Dll.Models.Account;
using PeopleComments.Dll.Entities;
using PeopleComments.Dll.Services;

namespace PeopleComments.Dll.Repositories.Account
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AccountCommentsContext _context;
        private readonly IMapper _mapper;


        public AccountRepository(
            AccountCommentsContext context,
            IMapper mapper
            )
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

        }


        public async Task<bool> AccountExistAsync (int accountId)
        {
            return await _context.Accounts.AnyAsync(c => c.Id == accountId);
        }


        public async Task<IEnumerable<Entities.Account>> GetAccountsAsync()
        {
            var collection = _context.Accounts as IQueryable<Entities.Account>;
            var collectionToReturn = await collection.OrderBy(c => c.Name)
                .ToListAsync();
            return collectionToReturn;
        }

        public async Task<(IEnumerable<Entities.Account>, PaginationMetaData)> GetAccountsAsync(
            string? name, string? searchQuery, int pageNumber, int pageSize)
        {
            var collection = _context.Accounts as IQueryable<Entities.Account>;

            if (!string.IsNullOrWhiteSpace(name))
            {
                name = name.Trim();
                collection = collection.Where(c => c.Name == name);
            }

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                collection = collection.Where(a => a.Name.Contains(searchQuery)
                    || a.Email != null && a.Email.Contains(searchQuery));
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

        public async Task<Entities.Account?> GetAccountAsync(int accountId)
        {
            return await _context.Accounts
                .Where(a => a.Id == accountId).FirstOrDefaultAsync();
            
        }

        public async Task<bool> AddAccount(Entities.Account account)
        {
            _context.Accounts.Add(account);
            await SaveChangesAsync();
            return true;
        }

        public void DeleteAccount(Entities.Account account)
        {
            _context.Accounts.Remove(account);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }

    }
}
