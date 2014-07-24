using MyBlog.Core.Helpers;
using MyBlog.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Data.MyBlogRepository
{
    public interface IMyBlogRepository
    {
        IEnumerable<Post> GetPostsByUser(MyBlogUser user,PaginationInfo paginationInfo);

        IEnumerable<Post> GetRecentPosts(PaginationInfo paginationInfo);

        IEnumerable<Core.Models.Post> GetPostsByMatchingTitle(string title, Core.Helpers.PaginationInfo paginationInfo);

        Post GetPost(int id);

        IEnumerable<Tag> GetAllTags();
        
        Tag GetTag(string name);

        IEnumerable<Core.Models.Tag> GetMatchingTags(string name);

        IEnumerable<Post> GetPostsByTag(Tag tag,PaginationInfo paginationInfo);

        IEnumerable<Post> GetPosts(PaginationInfo paginationInfo);

        IEnumerable<Post> GetPostsByTitle(String title,PaginationInfo paginationInfo);

        IEnumerable<Post> GetPostsByDate(DateTime date, PaginationInfo paginationInfo);

        Task<MyBlogUser> GetUser(string id);

        IEnumerable<Core.Models.MyBlogUser> GetUsers();

        Post CreatePost(Post post);

        Tag CreateTag(Tag tag);

        Post UpdatePost(Post post);

        void AddNewTagToPost(int postId, Tag tag);

        string DeletePost(int id);
    }
}
