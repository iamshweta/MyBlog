using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MyBlog.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Core.MyBlogDataContext
{
    public class MyBlogDbInitializer : CreateDatabaseIfNotExists<MyBlogDbContext>
    {

        protected override void Seed(MyBlogDbContext context)
        {
            var UserManager = new UserManager<MyBlogUser>(new


                                         UserStore<MyBlogUser>(context));

            //Create User=Admin with password=123456
            string name = "Admin";
            string password = "123456";
            var user = new MyBlogUser();
            user.UserName = name;
            //Post post = new Post();
            
            //post.Content = "Shweta's Blog"; 
            //post.Description = "Shweta's Blog";
            //post.ShortDescription = "Shweta's Blog";
            //post.PostedOn = System.DateTime.Today;
            //post.Title = "Shweta's Blog";
            //post.Url = "shweta/Blog";
            //post.PostedBy = user;          
            //post.Tags = new List<Tag>();
            //Tag t = new Tag();
            //t.Name = "Blog";
            //t.Url = "/Blog";
            //t.Posts = new List<Post>();
            //t.Posts.Add(post);
            //post.Tags.Add(t);

            //user.Posts = new List<Post>();
            //user.Posts.Add(post);
            var adminresult = UserManager.Create(user, password);

            //Add User Admin to Role Admin
            if (adminresult.Succeeded)
            {
              
            }
            base.Seed(context);
        }
    }
}

