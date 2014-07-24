using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Core.Models
{
     
    public class Post
    {
         
        public int Id { get; set; }
        
        public string Title { get; set; }
        
        public string Url { get; set; }
        
        public string ShortDescription { get; set; }
         
        public string Description { get; set; }
         
        public string Content { get; set; }
        
        public DateTime PostedOn { get; set; }
         
        public DateTime ModifiedOn { get; set; }

        public ICollection<Tag> Tags { get; set; }
        ///// <summary>
        ///// UID of the user by whom the Post is created
        ///// </summary>
        //public int PostedBy { get; set; } 
        public virtual  MyBlogUser PostedBy { get; set; } 

    }
}
