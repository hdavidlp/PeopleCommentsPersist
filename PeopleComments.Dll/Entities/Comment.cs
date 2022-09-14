using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace PeopleComments.Dll.Entities
{
    public class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]    
        public string CommentDetail { get; set; }


        // Account - Comments Relation 
        [ForeignKey("AccountId")]
        public Account? Account { get; set; }   
        public int AccountId { get; set; }  

       

        //public Comment(string comentDetail="")
        //{
        //    CommentDetail = comentDetail;
        //}


    }
}
