using PeopleComments.Dll.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PeopleComments.Dll.Models
{
    public class CommentForCreationDto
    {
        [Required(ErrorMessage = "You should provide a Comment value.")]
        [MaxLength(255)]
        public string CommentDetail { get; set; } = String.Empty;
    }
}
