using MyBlog.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyBlog.Models
{
    public class MyBlogsViewModel : BaseViewModel
    {
        public PostsListViewModel PostListModel { get; set; }
        public List<Tag> SearchTagsList { get; set; }

        public MyBlogsViewModel()
        {
            PostListModel = new PostsListViewModel();
            SearchTagsList = new List<Tag>();           
        }
    }
}