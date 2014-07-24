using MyBlog.Core.Models;
using MyBlog.Core.MyBlogDataContext;
using MyBlog.Data.MyBlogRepository;
using Serilog;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyBlog
{
    public static class ContainerBootstrapper
    {
        public static void BootstrapStructureMap()
        {

            // Initialize the static ObjectFactory container
            ObjectFactory.Initialize(x =>
            {
                x.For<IMyBlogRepository>().Use<MyBlogRepository>();
                x.For<Microsoft.AspNet.Identity.IUserStore<MyBlogUser>>()
                  .Use<Microsoft.AspNet.Identity.EntityFramework.UserStore<MyBlogUser>>();

                x.For<System.Data.Entity.DbContext>().Use<MyBlogDbContext>();
            });
        }
    }

    public class StructureMapControllerFactory : DefaultControllerFactory
    {
        protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext, Type controllerType)
        {
            try
            {
                if (requestContext == null || controllerType == null)
                    return null;
                return ObjectFactory.GetInstance(controllerType) as Controller;
            }
            catch (StructureMapException)
            {
                Log.Error("Object Factory has {0}", ObjectFactory.WhatDoIHave());
                throw;
            }
        }
    }
}