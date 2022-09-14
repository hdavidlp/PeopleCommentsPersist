using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using System.Diagnostics.CodeAnalysis;

namespace PeopleComments.Dll.Entities
{
    public class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }


        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        [MaxLength(50)]
        [AllowNull]
        public string Email { get; set; }

        public ICollection<Comment> Comments { get; set; }
                = new List<Comment>();

        public Account(string name)
        {
            Name = name;
        }


    }
}
