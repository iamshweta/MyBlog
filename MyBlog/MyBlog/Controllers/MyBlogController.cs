using MyBlog.Core.MyBlogDataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Linq;
using MyBlog.Core.Models;
using MyBlog.Data.MyBlogRepository;
using System.Web.Security;
using MyBlog.Filters;
using WebMatrix.WebData;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using MyBlog.Core.Helpers;
using MyBlog.Models;
using Serilog;
using Seq;
using MyBlog.LoggerModels;
using StructureMap;

namespace MyBlog.Controllers
{
    [RequireHttps]
    [HandleError(View="ErrorView")]
    [Authorize]
    public class MyBlogController : Controller
    {
        IMyBlogRepository repository;


        public MyBlogController(IMyBlogRepository repository)
        {
            this.repository = repository;
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            Log.Error("Exception Occured: {ExceptionMessage} {@StackTrace}", filterContext.Exception.Message, filterContext.Exception.StackTrace);
           base.OnException(filterContext);
        }

        public async Task<ActionResult> MyBlogs()
        {
            // Serilog-style message template, Name == "xyz"
            Log.Information("User logged in : {Name}", HttpContext.User.Identity.Name);

            MyBlogsViewModel model = new MyBlogsViewModel();
            PaginationInfo PageInfo = new PaginationInfo() { PageNo = 1, PageSize = 5 };
            PostsListViewModel postsListModel = await GetPostListForMyBlogs(PageInfo);
            model.PostListModel = postsListModel;
            var allTags = repository.GetAllTags();
            model.SearchTagsList = allTags != null ? allTags.ToList() : new List<Tag>();
            ViewBag.Title = "MyBlogs";
            ViewBag.ActionName = "MyBlogs";
            return View(model);
        }


        public ActionResult RecentBlogs(int pageNo = 1, int pageSize = 5)
        {
            MyBlogsViewModel model = new MyBlogsViewModel();
            PaginationInfo PageInfo = new PaginationInfo() { PageNo = pageNo, PageSize = pageSize };

            PostsListViewModel postsListModel = GetPostListModelForRecentBlogs(PageInfo);
            model.PostListModel = postsListModel;
            var allTags = repository.GetAllTags();
            model.SearchTagsList = allTags != null ? allTags.ToList() : new List<Tag>();
            ViewBag.Title = "Recent Blogs";
            ViewBag.ActionName = "RecentBlogs";          
            return View("MyBlogs", model);
        }



        public ActionResult Post(int year, int month, string title)
        {
            var post = repository.GetPostsByTitle(title, new PaginationInfo())
                .Where(x => x.PostedOn.Year == year && x.PostedOn.Month == month).FirstOrDefault();

            var postLog = Log.ForContext("PostId", post.Id);
            postLog.Information("Post {id} opened. {@post}", post.Id, post);

            return View(post);
        }

        public ActionResult Tag(string tag)
        {
            MyBlogsViewModel model = new MyBlogsViewModel();
            PaginationInfo PageInfo = new PaginationInfo() { PageNo = 1, PageSize = 5 };
            PostsListViewModel postsListModel = GetPostListModelForTag(tag, PageInfo);
            model.PostListModel = postsListModel;
            var allTags = repository.GetAllTags();
            model.SearchTagsList = allTags != null ? allTags.ToList() : new List<Tag>();
            ViewBag.Title = "Tag #" + tag;
            postsListModel.Header = "Blogs under Tag " + tag;
            ViewBag.ActionName = "Tag";
            return View("MyBlogs", model);
        }



        public async Task<ActionResult> Pagination(string actionName, int pageNo = 1, int pageSize = 5, string tag = "")
        {
            PostsListViewModel postsListModel = new PostsListViewModel();
            PaginationInfo PageInfo = new PaginationInfo() { PageNo = pageNo, PageSize = pageSize };

            ViewBag.ActionName = actionName;
            switch (actionName)
            {
                case "RecentBlogs":
                    postsListModel = GetPostListModelForRecentBlogs(PageInfo);
                    break;
                case "MyBlogs":
                    postsListModel = await GetPostListForMyBlogs(PageInfo);
                    break;
                case "Tag":
                    postsListModel = GetPostListModelForTag(tag, PageInfo);
                    break;
                default:
                    break;
            }
            return PartialView("_PostsListView", postsListModel);
        }

        public ActionResult GlobalSearch(string query = "")
        {
            PostsListViewModel postsListModel = new PostsListViewModel();

            //Log the search queries
            Log.Information("Search query : {query}", query);

            if (!string.IsNullOrEmpty(query))
            {
                postsListModel.Header = "Search Results";
                //1. Search By Title
                var matchingPosts = repository.GetPostsByMatchingTitle(query, new PaginationInfo());
                if (matchingPosts != null)
                {
                    postsListModel.Posts.AddRange(matchingPosts.ToList().Distinct());
                }
                //2. Search By Tags
                var matchingTags = repository.GetMatchingTags(query);
                if (matchingTags != null)
                {
                    foreach (var item in matchingTags)
                    {
                        if (item.Posts != null)
                        {
                            foreach (var post in item.Posts)
                            {
                                if (!postsListModel.Posts.Any(x => x.Id == post.Id))
                                {
                                    postsListModel.Posts.Add(post);
                                }
                            }

                        }
                    }
                }
            }
            else
            {
                postsListModel.Errors = new List<string>();
                postsListModel.Errors.Add("No query provided for search.");
            }
            return PartialView("_PostsListView", postsListModel);
        }

        public ActionResult Search()
        {
            ViewBag.Message = "Your Search page.";
            Log.Warning("Page under construction is accessed!!");
            return View();
        }

        public ActionResult NewBlog()
        {
            NewBlogViewModel model = new NewBlogViewModel();
            ViewBag.Title = "New Blog";
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> NewBlog(NewBlogViewModel postModel)
        {
            Post responsePost = new Post();
            Post post = new Post();
            if (postModel != null && postModel.post != null)
            {
                string inputText = HttpUtility.HtmlDecode(postModel.post.Content);
                post = postModel.post;
                post.Content = inputText;

                post.ModifiedOn = System.DateTime.Today;

                //populate user
                var user = await repository.GetUser(User.Identity.Name);
                post.PostedBy = user;

                //populate tags
                post.Tags = new List<Tag>();
                if (postModel.TagNames != null && postModel.TagNames.Any())
                {
                    foreach (var tagName in postModel.TagNames)
                    {
                        var tagModel = repository.GetTag(tagName);
                        if (tagModel == null)
                        {
                            tagModel = new Tag() { Id = 0, Name = tagName, Url = tagName };
                        }
                        post.Tags.Add(tagModel);
                    }
                }

                if (post.Id == 0)
                {
                    ViewBag.Title = "New Blog";
                    post.PostedOn = System.DateTime.Today;
                    responsePost = repository.CreatePost(post);

                    var postLog = Log.ForContext("PostId", responsePost.Id);
                    postLog.Information("Post {id} is created {@responsePost}", responsePost.Id, responsePost);
                }
                else
                {
                    ViewBag.Title = "Edit Blog";
                    responsePost = repository.UpdatePost(post);

                    var postLog = Log.ForContext("PostId", responsePost.Id);
                    postLog.Information("Post {id} is updated", responsePost.Id);
                }
            }
            return RedirectToAction("Post", new { year = responsePost.PostedOn.Year, month = responsePost.PostedOn.Month, title = responsePost.Title });
        }

        [HttpPost]
        public ActionResult EditBlog(int id)
        {
            NewBlogViewModel model = new NewBlogViewModel();
            var post = repository.GetPost(id);

            if (post != null)
            {
                model.post = post;
                model.TagNames = new List<string>();
                if (model.post.Tags != null && model.post.Tags.Any())
                {
                    foreach (var item in model.post.Tags)
                    {
                        model.TagNames.Add(item.Name);
                    }
                }
            }
            else
            {
                model.Errors = new List<string>();
                model.Errors.Add("Post not Found.");
            }

            return View("NewBlog", model);
        }

        public ActionResult DeleteBlog(int id)
        {
            string response = repository.DeletePost(id);


            var postLog = Log.ForContext("PostId", id);
            postLog.Information("Post {id} is deleted", id);

            return Content(response);
        }

        #region private methods
        private PostsListViewModel GetPostListModelForRecentBlogs(PaginationInfo PageInfo)
        {
            PostsListViewModel postsListModel = new PostsListViewModel();
            postsListModel.Header = "Recent Posts";
            postsListModel.Posts = repository.GetRecentPosts(PageInfo).ToList();
            postsListModel.TotalPosts = postsListModel.Posts.Count();            
            postsListModel.PageInfo = PageInfo;
            return postsListModel;
        }


        private async Task<PostsListViewModel> GetPostListForMyBlogs(PaginationInfo PageInfo)
        {
            var user = await repository.GetUser(User.Identity.Name);
            PostsListViewModel postsListModel = new PostsListViewModel();
            postsListModel.Header = "MyBlogs so far..";
            postsListModel.Posts = user.Posts != null ? user.Posts.ToList() : new List<Post>();
            postsListModel.TotalPosts = postsListModel.Posts.Count();
            postsListModel.Posts = postsListModel.Posts.Skip((PageInfo.PageNo - 1) * PageInfo.PageSize).Take(PageInfo.PageSize).ToList();
            postsListModel.PageInfo = PageInfo;          
            return postsListModel;
        }

        private PostsListViewModel GetPostListModelForTag(string tag, PaginationInfo PageInfo)
        {
            var tagModel = repository.GetTag(tag);
            PostsListViewModel postsListModel = new PostsListViewModel();
            postsListModel.Posts = tagModel.Posts != null ? tagModel.Posts.ToList() : new List<Post>();
            postsListModel.TotalPosts = postsListModel.Posts.Count();
            postsListModel.Posts = postsListModel.Posts.Skip((PageInfo.PageNo - 1) * PageInfo.PageSize).Take(PageInfo.PageSize).ToList();
            postsListModel.PageInfo = PageInfo;           
            postsListModel.Header = "Blogs under Tag " + tag;
            return postsListModel;
        }
        #endregion
    }
}
