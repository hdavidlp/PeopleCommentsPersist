using System.ComponentModel.DataAnnotations;

namespace PeopleComments.Dll.Models.Comment
{
    public class CommentForUpdateDto
    {
        //[Required(ErrorMessage = "You should provide a Comment value.")]
        [Required]
        [MaxLength(255)]
        public string? CommentDetail { get; set; }
    }
}
