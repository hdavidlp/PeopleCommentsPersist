
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;


namespace PeopleComments.Dll.Models.Account
{
    public class AccountForCreationDto
    {
        public int Id { get; set; }


        [Required]
        [MaxLength(30)]
        public string Name { get; set; } = String.Empty;    

        [MaxLength(50)]
        [AllowNull]
        public string Email { get; set; }

        public ICollection<Entities.Comment> Comments { get; set; }
                = new List<Entities.Comment>();

    }
}
