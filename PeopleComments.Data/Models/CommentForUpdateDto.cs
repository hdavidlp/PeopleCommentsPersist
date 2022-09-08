using System.ComponentModel.DataAnnotations;

namespace PeopleComments.Data.Models
{
    public class CommentForUpdateDto
    {
        //[Required(ErrorMessage = "You should provide a Comment value.")]
        [Required]
        [MaxLength(255)]
        public string? CommentDetail { get; set; } 
    }
}
