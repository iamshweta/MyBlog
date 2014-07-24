using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Linq;
using MyBlog.Core.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MyBlog.Core.MyBlogDataContext
{
    public class MyBlogDbContext : IdentityDbContext<MyBlogUser>
    {

        public DbSet<Post> Posts { get; set; }
        public DbSet<Tag> Tags { get; set; }

        public MyBlogDbContext()
            : base("DefaultConnection")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<MyBlogUser>().HasMany(u => u.Posts);
            modelBuilder.Entity<Post>().HasMany(p => p.Tags).
      WithMany(p => p.Posts).
      Map(
       m =>
       {
           m.MapLeftKey("Post_id");
           m.MapRightKey("Tag_id");
           m.ToTable("PostTagMap");
       });


            modelBuilder.Entity<IdentityUserLogin>().HasKey<string>(l => l.UserId);
            base.OnModelCreating(modelBuilder);
        }



    }
}
