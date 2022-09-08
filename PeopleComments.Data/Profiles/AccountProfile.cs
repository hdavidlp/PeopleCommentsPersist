using AutoMapper;
using PeopleComments.Data.Entities;
using PeopleComments.Data.Models;

namespace PeopleComments.Data.Profiles
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
