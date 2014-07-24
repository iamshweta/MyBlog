using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Serilog;
using Seq;
using Serilog.Core;

namespace MyBlog.LoggerModels
{
    public class UserNameEnricher : ILogEventEnricher
    {
        public void Enrich(Serilog.Events.LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var theProperty = propertyFactory.CreateProperty("UserName", HttpContext.Current.User.Identity.Name);
            logEvent.AddPropertyIfAbsent(theProperty);
        }
    }

    public class GlobalPropertyEnricher : ILogEventEnricher
    {
        public string Property { get; set; }
        public string Key { get; set; }
        public GlobalPropertyEnricher(string key, string value)
        {
            this.Property = value;
            this.Key = key;
        }

        public void Enrich(Serilog.Events.LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var theProperty = propertyFactory.CreateProperty(this.Key, this.Property);
            logEvent.AddPropertyIfAbsent(theProperty);
        }
    }
}