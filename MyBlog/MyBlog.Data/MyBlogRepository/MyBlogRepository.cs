using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MyBlog.Core.Helpers;
using MyBlog.Core.Models;
using MyBlog.Core.MyBlogDataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Data.MyBlogRepository
{
    public class MyBlogRepository : IMyBlogRepository
    {
        private MyBlogDbContext db;
        private UserManager<MyBlogUser> userManager;

        public MyBlogRepository()
        {
            db = new MyBlogDbContext();
            userManager = new UserManager<MyBlogUser>(new UserStore<MyBlogUser>(db));
        }

        public IEnumerable<Post> GetPostsByUser(MyBlogUser user, PaginationInfo paginationInfo)
        {
            return db.Posts.Where(x => x.PostedBy.Id == user.Id);
        }

        public IEnumerable<Post> GetRecentPosts(Core.Helpers.PaginationInfo paginationInfo)
        {
            var posts = db.Posts.OrderByDescending(x => x.PostedOn);
            if (posts != null)            {
              
                if (posts.Count() >= paginationInfo.PageSize)
                {
                    var responsePosts = posts.Skip((paginationInfo.PageNo - 1 ) * paginationInfo.PageSize).Take(paginationInfo.PageSize).ToList();
                    return responsePosts;
                }
                return posts;
            }
            return new List<Post>();
        }

        public Core.Models.Post GetPost(int id)
        {
            return db.Posts.Include("Tags").Where(x => x.Id == id).FirstOrDefault();
        }

        public IEnumerable<Core.Models.Tag> GetAllTags()
        {
            return db.Tags.OrderBy(x => x.Name);
        }

        public Core.Models.Tag GetTag(string name)
        {
            return db.Tags.Include("Posts").Where(x => x.Name.Equals(name,StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
        }

        public IEnumerable<Core.Models.Tag> GetMatchingTags(string name)
        {
            return db.Tags.Include("Posts").Where(x => x.Name.ToUpper().Contains(name.ToUpper()));
        }

        public IEnumerable<Core.Models.Post> GetPostsByTag(Core.Models.Tag tag, Core.Helpers.PaginationInfo paginationInfo)
        {
            return db.Tags.Where(x => x.Id == tag.Id).FirstOrDefault().Posts;
        }

        public IEnumerable<Core.Models.Post> GetPosts(Core.Helpers.PaginationInfo paginationInfo)
        {
            return db.Posts.OrderBy(x => x.Title);
        }

        public IEnumerable<Core.Models.Post> GetPostsByTitle(string title, Core.Helpers.PaginationInfo paginationInfo)
        {
            return db.Posts.Include("Tags").Where(x => x.Title.Equals(title,StringComparison.InvariantCultureIgnoreCase));
        }

        public IEnumerable<Core.Models.Post> GetPostsByMatchingTitle(string title, Core.Helpers.PaginationInfo paginationInfo)
        {
            return db.Posts.Where(x => x.Title.ToUpper().Contains(title.ToUpper()));
        }

        public IEnumerable<Core.Models.Post> GetPostsByDate(DateTime date, Core.Helpers.PaginationInfo paginationInfo)
        {
            return db.Posts.Where(x => x.PostedOn == date);
        }

        public async Task<Core.Models.MyBlogUser> GetUser(string id)
        {
            return await userManager.FindByNameAsync(id);
        }

        public IEnumerable<Core.Models.MyBlogUser> GetUsers()
        {
            return  userManager.Users;
        }

        public Post CreatePost(Core.Models.Post post)
        {
            var outputPost = db.Posts.Add(post);
            db.SaveChanges();
            return outputPost;
        }

        public Tag CreateTag(Tag tag)
        {
            var outputTag = db.Tags.Add(tag);
            db.SaveChanges();
            return outputTag;
        }

        public void AddNewTagToPost(int postId, Tag tag)
        {
            var outputTag = CreateTag(tag);
            var existingPost = db.Posts.Where(x => x.Id == postId).FirstOrDefault();
            existingPost.Tags.Add(outputTag);
            db.SaveChanges();            
        }

        public Post UpdatePost(Core.Models.Post post)
        {
            var existingPost = db.Posts.Where(x => x.Id == post.Id).FirstOrDefault();
            foreach (var tag in post.Tags)
            {
                if (!existingPost.Tags.Any(t => t.Id == tag.Id))
                {
                    var existingTag = GetTag(tag.Name);
                    if (existingTag != null)
                        existingPost.Tags.Add(existingTag);
                    else
                        AddNewTagToPost(existingPost.Id, tag);
                    db.SaveChanges();
                }
            }
            if (post.PostedOn == DateTime.MinValue)
            {
                post.PostedOn = existingPost.PostedOn;
            }
            var entry = db.Entry(existingPost);
            entry.OriginalValues.SetValues(existingPost);
            entry.CurrentValues.SetValues(post);
            db.SaveChanges();
            return post;
        }

        public string DeletePost(int id)
        {
            var existingPost = db.Posts.Where(x => x.Id == id).FirstOrDefault();
            db.Posts.Remove(existingPost);
            db.SaveChanges();
            return "success";
        }
    }
}
