using PeopleComments.Dll.Entities;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace PeopleComments.Dll.Models.Account
{
    public class AccountForUpdateDto
    {

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        [MaxLength(50)]
        [AllowNull]
        public string Email { get; set; }

        //public ICollection<Comment> Comments { get; set; }
        //        = new List<Comment>();


    }
}
