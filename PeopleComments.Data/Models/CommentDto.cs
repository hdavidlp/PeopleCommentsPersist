using PeopleComments.Data.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PeopleComments.Data.Models
{
    public class CommentDto
    {
        public int Id { get; set; }
        
        public string CommentDetail { get; set; } = string.Empty;
    }
}
