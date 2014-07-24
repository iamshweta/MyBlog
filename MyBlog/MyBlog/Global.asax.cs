using MyBlog.Core.MyBlogDataContext;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Serilog;
using Seq.Client.Serilog;
using Serilog.Events;
using MyBlog.LoggerModels;
using MyBlog.Core.Models;


namespace MyBlog
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Database.SetInitializer<MyBlogDbContext>(new MyBlogDbInitializer());

            ContainerBootstrapper.BootstrapStructureMap();

            //change the default controller factory  
            ControllerBuilder.Current.SetControllerFactory(new StructureMapControllerFactory());

            Log.Logger = new LoggerConfiguration().ReadAppSettings()
               // .WriteTo.ColoredConsole(outputTemplate: "[{UserName}]")
               .Enrich.With(new GlobalPropertyEnricher("key","value"))
               .Destructure.ByTransforming<Post>(p => new { Title = p.Title, Id = p.Id, PostedOn = p.PostedOn, PostedBy = p.PostedBy })
               //.WriteTo.Seq("http://localhost:5341")
               //.WriteTo.RollingFile(@"E:\Log-{Date}.txt", restrictedToMinimumLevel: LogEventLevel.Warning)
               .CreateLogger();

        }
    }
}