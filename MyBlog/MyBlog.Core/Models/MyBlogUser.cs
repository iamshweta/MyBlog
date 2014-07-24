using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Core.Models
{
    public class MyBlogUser : IdentityUser
    {
        public virtual ICollection<Post> Posts { get; set; }
          
    }
}
