using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebApplicationppp
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("fonts*.woff");
            routes.IgnoreRoute("*.js");
            routes.IgnoreRoute("*.html");
            routes.IgnoreRoute("*.css");
            routes.IgnoreRoute("api/*");
            routes.IgnoreRoute("elmah.axd");

            routes.MapRoute(
            name: "index",
            url: "Admin/Index",
            defaults: new { controller = "Admin", action = "Index" });

            routes.MapRoute(
            name: "Reservations",
            url: "Admin/Reservations",
            defaults: new { controller = "Admin", action = "Reservations" });

            routes.MapRoute(
            name: "Settings",
            url: "Admin/Settings",
            defaults: new { controller = "Admin", action = "Settings" });

            routes.MapRoute(
            name: "Schedule",
            url: "Admin/Schedule",
            defaults: new { controller = "Admin", action = "Schedule" });

            routes.MapRoute(
            name: "Info",
            url: "Admin/Info",
            defaults: new { controller = "Admin", action = "Info" });

            routes.MapRoute(
            name: "Users",
            url: "Admin/Users",
            defaults: new { controller = "Admin", action = "Users" });

            routes.MapRoute(
            name: "Register",
            url: "Acc/Register",
            defaults: new { controller = "Acc", action = "Register" });

            routes.MapRoute(
            name: "Login",
            url: "Acc/Login",
            defaults: new { controller = "Acc", action = "Login" });

            routes.MapRoute(
            name: "ChangePassword",
            url: "Acc/ChangePassword",
            defaults: new { controller = "Acc", action = "ChangePassword" });

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{*url}",
            //    defaults: new { controller = "Home", action = "Index" });

            // this should do the job on F5
            routes.MapRoute(
                name: "Default",
                url: "{dummyController}/{dummyAction}/{dummy1}/{dummy2}/{dummy3}",
                defaults: new
                {
                    controller = "Home",
                    action = "Index"
                    ,
                    dummyController = UrlParameter.Optional
                    ,
                    dummyAction = UrlParameter.Optional
                    ,
                    dummy1 = UrlParameter.Optional
                    ,
                    dummy2 = UrlParameter.Optional
                    ,
                    dummy3 = UrlParameter.Optional
                });

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);
        }
    }
}
