using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyBlog.Models
{
    public class BaseViewModel
    {
        public List<string> Errors { get; set; }
        public BaseViewModel()
        {
            Errors = new List<string>();
        }
    }
}