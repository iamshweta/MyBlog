using MyBlog.Core.Helpers;
using MyBlog.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyBlog.Models
{
    public class PostsListViewModel : BaseViewModel
    {
        public List<Post> Posts { get; set; }
        public int TotalPosts { get; set; }
        public string Header { get; set; }
        public bool ShowPrevious
        {
            get
            {
                return (PageInfo.PageNo != 1);
            }
        }
        public bool ShowNext {
            get
            {
                double factor = (double)TotalPosts / (double)PageInfo.PageSize;
                return ( factor > PageInfo.PageNo);
            }
        }
        public PaginationInfo PageInfo { get; set; }
        public string Tag { get; set; }
        public PostsListViewModel()
        {
            Posts = new List<Post>();
            TotalPosts = 0;
            Header = string.Empty;            
            PageInfo = new PaginationInfo() { PageNo = 1, PageSize = 5 };
        }
    }
}