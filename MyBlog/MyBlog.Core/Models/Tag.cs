using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Core.Models
{
    public class Tag
    {
        public int Id { get; set; }
       
        public string Name { get; set; }
        
        public string Url { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
    }
}
