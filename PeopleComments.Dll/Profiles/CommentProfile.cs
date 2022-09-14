using AutoMapper;


namespace PeopleComments.Dll.Profiles
{
    public class CommentProfile:Profile
    {
        public CommentProfile()
        {
            CreateMap<Entities.Comment, Models.CommentDto>();
            CreateMap<Entities.Comment, Models.CommentForUpdateDto>();
            CreateMap<Models.CommentDto, Entities.Comment>();
            CreateMap<Models.CommentForCreationDto, Entities.Comment>();
            CreateMap<Models.CommentForUpdateDto, Entities.Comment>(); 

        }
    }
}
