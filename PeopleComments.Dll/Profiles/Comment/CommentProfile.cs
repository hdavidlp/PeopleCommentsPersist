using AutoMapper;
using PeopleComments.Dll.Models.Comment;


namespace PeopleComments.Dll.Profiles.Comment
{
    public class CommentProfile : Profile
    {
        public CommentProfile()
        {
            CreateMap<Entities.Comment, CommentDto>();
            CreateMap<Entities.Comment, CommentForCreationDto>();
            CreateMap<CommentDto, Entities.Comment>();
            CreateMap<CommentForCreationDto, Entities.Comment>();
            CreateMap<CommentForUpdateDto, Entities.Comment>();

        }
    }
}
