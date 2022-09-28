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
    }
}
