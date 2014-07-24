using MyBlog.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyBlog.Models
{
    public class NewBlogViewModel : BaseViewModel
    {
        public Post post { get; set; }
        public List<string> TagNames { get; set; }

        public NewBlogViewModel()
        {
            post = new Post();
            TagNames = new List<string>();
        }
        
    }
}