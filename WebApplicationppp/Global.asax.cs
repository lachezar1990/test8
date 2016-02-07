using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;
using System.Web.Optimization;

namespace WebApplicationppp
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            GlobalConfiguration.Configuration.Formatters.Remove(GlobalConfiguration.Configuration.Formatters.XmlFormatter);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            BundleConfig.RegisterBundles(BundleTable.Bundles);
            GlobalConfiguration.Configuration.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
        }

        //protected void Application_BeginRequest(object sender, EventArgs e)
        //{
        //    if (Context.Request.Path.Contains("api/") && Context.Request.HttpMethod == "OPTIONS")
        //    {

        //        Context.Response.AddHeader("Access-Control-Allow-Origin", "*");
        //        Context.Response.AddHeader("Access-Control-Allow-Methods", "GET, POST PUT, DELETE, OPTIONS");
        //        Context.Response.AddHeader("Access-Control-Allow-Credentials", "true");
        //        Context.Response.End();
        //    }
        //} 
    }
}