using AutoMapper;
using PeopleComments.Dll.Entities;
using PeopleComments.Dll.Models;

namespace PeopleComments.Dll.Profiles
{
    public class AccountProfile: Profile
    {

        public AccountProfile()
        {
            CreateMap<Entities.Account, Models.AccountWithoutCommentsDto>();
            CreateMap<Models.AccountForCreationDto, Entities.Account>();
            CreateMap<Models.AccountForUpdateDto, Entities.Account>();
            CreateMap<Entities.Account, Models.AccountForUpdateDto>();
        }
    }
}
