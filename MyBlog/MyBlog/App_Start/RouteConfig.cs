using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MyBlog
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

     
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "MyBlog", action = "MyBlogs", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                    "Action",
                    "{action}",
                     new { controller = "MyBlog", action = "Posts" }
                );

            routes.MapRoute(
                    "Tag",
                    "Tag/{tag}",
                    new { controller = "MyBlog", action = "Tag" }
                );
        }
    }
}