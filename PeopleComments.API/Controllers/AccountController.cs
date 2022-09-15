using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using PeopleComments.Dll.Entities;
using PeopleComments.Dll.Models.Account;
using PeopleComments.Dll.Services;
using System.Security.Principal;
using System.Text.Json;
//using Newtonsoft.Json;

namespace PeopleComments.API.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private const int maxPageSize = 10;
        private readonly IAccountCommentInfoRepository _accountCommentInfoRepository;
        private readonly IMapper _mapper;

        public AccountController(IAccountCommentInfoRepository accountCommentInfoRepository, IMapper mapper)
        {
            _accountCommentInfoRepository = accountCommentInfoRepository ??
                throw new ArgumentNullException(nameof(accountCommentInfoRepository));
            _mapper = mapper ?? throw
                new ArgumentNullException(nameof(mapper));
        }


        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<AccountWithoutCommentsDto>>> GetAccounts()
        {
            var accounts = await _accountCommentInfoRepository.GetAccountsAsync();
            return Ok(_mapper.Map<IEnumerable<AccountWithoutCommentsDto>>(accounts));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccountWithoutCommentsDto>>> GetAccounts(
            string? name, string? searchQuery, int pageNumber = 1, int pageSize = 10  )
        {
            if (pageSize > maxPageSize) pageSize = maxPageSize;

            var (accountEntities, paginationMetaData) = await _accountCommentInfoRepository
                .GetAccountsAsync(name, searchQuery, pageNumber, pageSize);

            Response.Headers.Add("X-Pagination",
                    JsonSerializer.Serialize(paginationMetaData));

            return Ok(_mapper.Map<IEnumerable<AccountWithoutCommentsDto>>(accountEntities));
        }


        [HttpGet("{id}", Name = "GetAccount")]
        public async Task<ActionResult<Account>> GetAccount(int id)
        {
            var account = await _accountCommentInfoRepository.GetAccountAsync(id);
            if (account == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<AccountWithoutCommentsDto>(account));
        }

        [HttpPost]
        public async Task<ActionResult<AccountForCreationDto>> CreateAccount(
            AccountForCreationDto account
            )
        {
            var newAccount = _mapper.Map<Account>(account);

            await _accountCommentInfoRepository.AddAccount(newAccount);
            await _accountCommentInfoRepository.SaveChangesAsync();

            var createdAccountToReturn =
                _mapper.Map<AccountWithoutCommentsDto>(newAccount);

            return Ok(_mapper.Map<AccountWithoutCommentsDto>(newAccount));
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> ApdateAccount(
            int id,
            AccountForUpdateDto account)
        {
            var selectedAccount = await _accountCommentInfoRepository.GetAccountAsync(id);

            if (selectedAccount == null)
            {
                return NotFound();
            }
            
            //_mapper.Map(source, destination)
            _mapper.Map(account, selectedAccount);

            await _accountCommentInfoRepository.SaveChangesAsync();
            return NoContent();
        }


        [HttpPatch("{id}")]
        public async Task<ActionResult> PartiallyUpdateAccount(
            int id,
            JsonPatchDocument<AccountForUpdateDto> patchData)
        {
            var account = await _accountCommentInfoRepository.GetAccountAsync(id);

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

            await _accountCommentInfoRepository.SaveChangesAsync();
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAccount(
            int id)
        {
            var accountEntity = await _accountCommentInfoRepository.GetAccountAsync(id);

            if (accountEntity == null)
            {
                return NotFound();
            }

            _accountCommentInfoRepository.DeleteAccount(accountEntity);
            await _accountCommentInfoRepository.SaveChangesAsync();

            return NoContent();
        }
    }
}
