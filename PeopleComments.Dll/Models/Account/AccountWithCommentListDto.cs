using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleComments.Dll.Models.Account
{
    public  class AccountWithCommentListDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public string CommentDetail { get; set; } = string.Empty;
        public DateTime Date { get; set; }  

        
        
    }
}
