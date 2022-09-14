using PeopleComments.Dll.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PeopleComments.Dll.Models
{
    public class CommentDto
    {
        public int Id { get; set; }
        
        public string CommentDetail { get; set; } = string.Empty;
    }
}
