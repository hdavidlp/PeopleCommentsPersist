using AutoMapper;
using PeopleComments.Dll.Entities;
using PeopleComments.Dll.Models.Account;

namespace PeopleComments.Dll.Profiles.Account
{
    public class AccountProfile : Profile
    {

        public AccountProfile()
        {
            CreateMap<Entities.Account, AccountWithoutCommentsDto>();
            CreateMap<AccountForCreationDto, Entities.Account>();
            CreateMap<AccountForUpdateDto, Entities.Account>();
            CreateMap<Entities.Account, AccountForUpdateDto>();
        }
    }
}
