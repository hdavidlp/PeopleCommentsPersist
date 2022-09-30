using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using PeopleComments.Dll.Entities;
using PeopleComments.Dll.Models.Account;
using PeopleComments.Dll.Repositories.Account;
using PeopleComments.Dll.Services.Account;
using System.Net.WebSockets;
using System.Security.Principal;
using System.Text.Json;
//using Newtonsoft.Json;

namespace PeopleComments.API.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;

        public AccountController(
            IMapper mapper,
            IAccountService accountService
            )
        {
            _mapper = mapper ?? throw
                new ArgumentNullException(nameof(mapper));
            _accountService = accountService ?? 
                throw new ArgumentNullException(nameof(accountService));
        }


        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<AccountWithoutCommentsDto>>> GetAccounts()
        {
            //var accounts = await _accountService.GetAccountsAsync();
            var accounts = await _accountService.GetAccountsWithoutCommentsAsync();
            return Ok(accounts);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccountWithoutCommentsDto>>> GetAccounts(
            string? name, string? searchQuery, int pageNumber = 1, int pageSize = 10  )
        {
            var (accountEntities, paginationMetaData) = await _accountService
                .GetAccountsAsync(name, searchQuery, pageNumber, pageSize);

            Response.Headers.Add("X-Pagination",
                    JsonSerializer.Serialize(paginationMetaData));

            return Ok(_mapper.Map<IEnumerable<AccountWithoutCommentsDto>>(accountEntities));
        }


        [HttpGet("{id}", Name = "GetAccount")]
        public async Task<ActionResult<Account>> GetAccount(int id)
        {
            var account = await _accountService.GetAccountAsync(id);

            if (account == null) return NotFound();
            return Ok(_mapper.Map<AccountWithoutCommentsDto>(account));
        }

        [HttpPost]
        public async Task<ActionResult<AccountForCreationDto>> CreateAccount(
            AccountForCreationDto account
            )
        {
            var newAccount = _mapper.Map<Account>(account);
            
            if (await _accountService.AddAccount(newAccount))
                return Ok(_mapper.Map<AccountWithoutCommentsDto>(newAccount));
            else
                return BadRequest();
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAccount(
            int id,
            AccountForUpdateDto account)
        {
            bool updateComplete = await _accountService.UpdateAccount(id, account);

            if (updateComplete) return NoContent();
            else return NotFound();
        }


        [HttpPatch("{id}")]
        public async Task<ActionResult> PartiallyUpdateAccount(
            int id,
            
            JsonPatchDocument<AccountForUpdateDto> patchData)
        {
            var account = await _accountService.GetAccountAsync(id);

            if (account == null) 
            {
                return NotFound();
            }

            var accountToPatch = _mapper.Map<AccountForUpdateDto>(account);
            patchData.ApplyTo(accountToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!TryValidateModel(accountToPatch))
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(accountToPatch, account);

            await _accountService.SaveChangesAsync();
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAccount(
            int id)
        {
            var accountEntity = await _accountService.GetAccountAsync(id);

            if (accountEntity == null)
            {
                return NotFound();
            }

            bool deleteSuccess = await _accountService.DeleteAccount(accountEntity);

            return NoContent();
        }
    }
}
