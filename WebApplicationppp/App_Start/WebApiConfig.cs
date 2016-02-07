using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Net.Http.Headers;
using System.Web.Http.Cors;
using System.Net.Http.Formatting;
using Newtonsoft.Json.Serialization;
using Elmah.Contrib.WebApi;
using System.Web.Http.ExceptionHandling;

namespace WebApplicationppp
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // enable elmah
            config.Services.Add(typeof(IExceptionLogger), new ElmahExceptionLogger());

            // Web API configuration and services
            //Enable CORS for all origins, all headers, and all methods,
            // You can customize this to match your requirements
            // Enabling CORS Globally
            var cors = new EnableCorsAttribute("*", "*", "*", exposedHeaders: "Has-More") { SupportsCredentials = true };
            //var cors2 = new EnableCorsAttribute("*", "*", "*", exposedHeaders: "UserRoles") { SupportsCredentials = true };

            cors.ExposedHeaders.Add("Has-More");
            //cors.ExposedHeaders.Add("UserRoles");

            config.EnableCors(cors);
            //config.EnableCors(cors2);
            //config.EnableCors();
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // By default Web Api wants to return XML.  (This is confusing because RestFul is based on JSON)
            // This line changes the default return type to JSON.
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
            //var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            //jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            config.Filters.Add(
            new ElmahErrorAttribute()
        );
        }

        public class ElmahErrorAttribute : System.Web.Http.Filters.ExceptionFilterAttribute
        {
            public override void OnException(
                System.Web.Http.Filters.HttpActionExecutedContext actionExecutedContext)
            {

                if (actionExecutedContext.Exception != null)
                    Elmah.ErrorSignal.FromCurrentContext().Raise(actionExecutedContext.Exception);

                base.OnException(actionExecutedContext);
            }
        }
    }
}
