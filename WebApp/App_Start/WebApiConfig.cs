using System.Web.Http;

namespace WebApp.App_Start
{
    public class WebApiConfig : System.Web.HttpApplication
    {

        public static void Register(HttpConfiguration config)
        {


            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { action = RouteParameter.Optional, id = RouteParameter.Optional}
            );
        }
    }
}